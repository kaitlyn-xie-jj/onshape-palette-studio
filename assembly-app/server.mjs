/** Single-process OAuth read-only Assembly preview. No CAD write endpoints are implemented. */
import http from 'node:http';
import { readFileSync } from 'node:fs';
import { dirname, join } from 'node:path';
import { fileURLToPath } from 'node:url';
import { randomBytes } from 'node:crypto';
import { parseAssemblyLocator, planAssembly } from './lib/planner.mjs';

const DIR = dirname(fileURLToPath(import.meta.url));
const paletteData = JSON.parse(readFileSync(join(DIR, '..', 'palettes.json'),'utf8'));
const demoData = JSON.parse(readFileSync(join(DIR, 'fixtures', 'assembly.json'),'utf8'));
const MIME = {'.html':'text/html; charset=utf-8', '.js':'text/javascript; charset=utf-8', '.css':'text/css; charset=utf-8'};
const STATIC = new Map([['/', 'index.html'],['/app.js','app.js'],['/app.css','app.css']]);
const MAX_SESSION_AGE = 8*60*60*1000;
function send(res, status, payload, contentType='application/json; charset=utf-8') {
  const body = typeof payload==='string' ? payload : JSON.stringify(payload);
  res.writeHead(status, {'Content-Type':contentType,'Cache-Control':'no-store','X-Content-Type-Options':'nosniff'});
  res.end(body);
}
function cookies(req) {
  return Object.fromEntries((req.headers.cookie||'').split(';').map(x=>x.trim().split('=').slice(0,2)).filter(x=>x.length===2));
}
function safeError(err) {
  // Never echo API response body, OAuth errors, credentials or tokens to the browser.
  return err instanceof InputError ? err.message : 'Onshape request failed. Check app permissions, linked references, and server logs.';
}
class InputError extends Error {}

export function createServer({env=process.env, fetchImpl=fetch, palettes=paletteData, demo=demoData}={}) {
  const sessions = new Map();
  const publicOrigin = env.PUBLIC_ORIGIN || 'http://127.0.0.1:8787';
  const origin = new URL(publicOrigin);
  if (origin.pathname !== '/' || origin.search || origin.hash || !['http:','https:'].includes(origin.protocol)) throw new Error('Invalid PUBLIC_ORIGIN');
  const secureCookie = origin.protocol === 'https:';
  const oauthId = env.ONSHAPE_CLIENT_ID || '';
  const oauthSecret = env.ONSHAPE_CLIENT_SECRET || '';
  const callback = `${origin.origin}/auth/callback`;
  const oauthBase = 'https://oauth.onshape.com';
  const apiBase = 'https://cad.onshape.com';
  const headers = {
    'Content-Security-Policy': "default-src 'self'; frame-ancestors 'self' https://cad.onshape.com; object-src 'none'; base-uri 'none'; form-action 'self' https://oauth.onshape.com",
    'Referrer-Policy':'no-referrer',
    'X-Content-Type-Options':'nosniff',
  };
  const tokenExpiry = t => Date.now() + Math.max(0, Number(t.expires_in || 3600) - 60)*1000;
  function setCookie(res, sessionId) {
    const opts = [`palette_session=${sessionId}`,'Path=/','HttpOnly',`Max-Age=${MAX_SESSION_AGE/1000}`,secureCookie?'Secure':'',secureCookie?'SameSite=None':'SameSite=Lax'];
    res.setHeader('Set-Cookie',opts.filter(Boolean).join('; '));
  }
  function clearCookie(res) { res.setHeader('Set-Cookie','palette_session=; Path=/; HttpOnly; Max-Age=0; SameSite=Lax'); }
  function existingSession(req) {
    const sid=cookies(req).palette_session;
    const s=sid && sessions.get(sid);
    if (!s) return null;
    if (Date.now()-s.created>MAX_SESSION_AGE) {sessions.delete(sid);return null;}
    return s;
  }
  function newSession(res) {
    const sid=randomBytes(24).toString('hex');
    const s={sid,created:Date.now(),state:null,token:null};
    sessions.set(sid,s);setCookie(res,sid);return s;
  }
  async function exchange(body) {
    const reply=await fetchImpl(`${oauthBase}/oauth/token`,{
      method:'POST',headers:{'Content-Type':'application/x-www-form-urlencoded','Accept':'application/json'},
      body:new URLSearchParams({...body,client_id:oauthId,client_secret:oauthSecret}).toString(),
      redirect:'manual',signal:AbortSignal.timeout(15000),
    });
    if (!reply.ok) throw new Error(`OAuth token exchange status ${reply.status}`);
    const data=await reply.json();
    if (typeof data.access_token!=='string') throw new Error('Missing OAuth access token');
    return data;
  }
  async function accessToken(s) {
    if (!s?.token?.access_token) throw new InputError('Connect your Onshape account first.');
    if (Date.now() < s.token.expiresAt) return s.token.access_token;
    if (!s.token.refresh_token) throw new InputError('Session expired; connect Onshape again.');
    const refreshed=await exchange({grant_type:'refresh_token',refresh_token:s.token.refresh_token});
    s.token={...refreshed,refresh_token:refreshed.refresh_token || s.token.refresh_token,expiresAt:tokenExpiry(refreshed)};
    return s.token.access_token;
  }
  async function readAssembly(s, loc) {
    const token=await accessToken(s);
    const path=`/api/v9/assemblies/d/${loc.did}/${loc.wvm}/${loc.wvmid}/e/${loc.eid}`;
    const url=new URL(path,apiBase);
    for (const [k,v] of Object.entries({includeMateFeatures:'false', includeNonSolids:'false', includeMateConnectors:'false', excludeSuppressed:'false'})) url.searchParams.set(k,v);
    const resp=await fetchImpl(url.toString(),{headers:{'Authorization':`Bearer ${token}`,'Accept':'application/json'},redirect:'manual',signal:AbortSignal.timeout(15000)});
    if (!resp.ok) throw new Error(`Onshape API status ${resp.status}`);
    if (!String(resp.headers.get('content-type')||'').includes('json')) throw new Error('API returned non-JSON');
    return resp.json();
  }
  const server = http.createServer(async (req,res)=>{
    for(const [k,v] of Object.entries(headers)) res.setHeader(k,v);
    let u;
    try {u=new URL(req.url||'/',publicOrigin);} catch {send(res,400,{error:'Invalid request URL'});return;}
    if (u.origin!==origin.origin) {send(res,400,{error:'Unexpected origin'});return;}
    if (req.method==='GET' && STATIC.has(u.pathname)) {
      const name=STATIC.get(u.pathname); const ext=name.slice(name.lastIndexOf('.'));
      return send(res,200,readFileSync(join(DIR,'public',name),'utf8'),MIME[ext]);
    }
    if (req.method==='GET' && u.pathname==='/api/palettes') return send(res,200,palettes);
    if (req.method==='GET' && u.pathname==='/api/status') return send(res,200, {configured:!!(oauthId&&oauthSecret),authenticated:!!existingSession(req)?.token,mode:'READ_ONLY'});
    if (req.method==='GET' && u.pathname==='/api/demo') {
      try {return send(res,200,planAssembly(demo,palettes,Object.fromEntries(u.searchParams)));}
      catch (e) {return send(res,400,{error:safeError(new InputError(e.message))});}
    }
    if (req.method==='GET' && u.pathname==='/auth/start') {
      if (!oauthId || !oauthSecret) return send(res,503,{error:'Configure ONSHAPE_CLIENT_ID and ONSHAPE_CLIENT_SECRET in your environment.'});
      const s=existingSession(req)||newSession(res);
      s.state=randomBytes(24).toString('hex');
      const authorize=new URL(`${oauthBase}/oauth/authorize`);
      for(const [k,v] of Object.entries({response_type:'code',client_id:oauthId,redirect_uri:callback,state:s.state,scope:'OAuth2Read'}))authorize.searchParams.set(k,v);
      res.writeHead(302,{Location:authorize.toString(),'Cache-Control':'no-store'});res.end();return;
    }
    if (req.method==='GET' && u.pathname==='/auth/callback') {
      const s=existingSession(req);
      const state=u.searchParams.get('state');
      if (!s?.state || !state || s.state!==state || !u.searchParams.get('code')) return send(res,403,{error:'OAuth state/callback verification failed.'});
      s.state=null; // single use even if token request fails
      try {
        const token=await exchange({grant_type:'authorization_code',code:u.searchParams.get('code'),redirect_uri:callback});
        s.token={...token,expiresAt:tokenExpiry(token)};
        res.writeHead(303,{Location:'/', 'Cache-Control':'no-store'});res.end();return;
      } catch(err) {return send(res,502,{error:safeError(err)});}
    }
    if (req.method==='GET' && u.pathname==='/api/plan') {
      try {
        const s=existingSession(req); if (!s?.token) return send(res,401,{error:'Connect your Onshape account first.'});
        let loc;
        try { loc=parseAssemblyLocator(u.searchParams.get('url')); }
        catch (err) { throw new InputError(err.message); }
        const def=await readAssembly(s,loc);
        const opts=Object.fromEntries(u.searchParams);
        const plan=planAssembly(def,palettes,opts);
        return send(res,200,{...plan,assembly:loc});
      } catch(err) {return send(res,err instanceof InputError ? 400:502,{error:safeError(err)});}
    }
    if (req.method==='POST' && u.pathname==='/api/apply') return send(res,501,{error:'Disabled: no verified Onshape appearance-write API integration. This preview never modifies CAD.'});
    if (req.method==='POST' && u.pathname==='/auth/logout') {
      const sid=cookies(req).palette_session;if(sid)sessions.delete(sid);clearCookie(res);return send(res,200,{ok:true});
    }
    return send(res,404,{error:'Not found'});
  });
  return server;
}

if (process.argv[1] && fileURLToPath(import.meta.url)===process.argv[1]) {
  const port=Number(process.env.PORT || 8787);
  const host=process.env.HOST || '127.0.0.1';
  createServer().listen(port,host,()=>console.log(`Assembly Palette Studio: http://${host}:${port} (read-only)`));
}
