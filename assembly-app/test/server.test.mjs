import { test } from 'node:test';
import assert from 'node:assert/strict';
import { createServer } from '../server.mjs';

async function withServer(options, testFn) {
 const server = createServer(options);
 await new Promise(resolve => server.listen(0,'127.0.0.1',resolve));
 const url = `http://127.0.0.1:${server.address().port}`;
 try {await testFn(url);} finally {await new Promise((res,rej)=>server.close(err=>err?rej(err):res()));}
}
const palette = {MONOCHROME:{name:'Monochrome',colors:Array.from({length:12},(_,i)=>`#${String(i).padStart(6,'0')}`)}};
const fixture={rootAssembly:{instances:[{id:'p1',type:'Part',partId:'p',name:'Name',documentId:'aaaaaaaaaaaaaaaaaaaaaaaa',elementId:'bbbbbbbbbbbbbbbbbbbbbbbb'}]},occurrences:[{path:['p1']}]};
const doc='https://cad.onshape.com/documents/aaaaaaaaaaaaaaaaaaaaaaaa/w/bbbbbbbbbbbbbbbbbbbbbbbb/e/cccccccccccccccccccccccc';

test('demo, status, no authenticated reads, and no model writes', async()=>{
 await withServer({palettes:palette,demo:fixture,env:{}},async base=>{
  const demo=await fetch(base+'/api/demo?theme=MONOCHROME').then(r=>r.json());
  assert.equal(demo.mode,'DRY_RUN_ONLY');
  assert.equal(demo.uniqueSourceCount,1);
  assert.equal((await fetch(base+'/api/status').then(r=>r.json())).authenticated,false);
  assert.equal((await fetch(base+'/api/plan?url='+encodeURIComponent(doc))).status,401);
  assert.equal((await fetch(base+'/auth/start')).status,503);
  assert.equal((await fetch(base+'/api/apply',{method:'POST'})).status,501);
 });
});

test('OAuth state is cookie-bound; read uses fixed host and Bearer; no token leaks',async()=>{
 let oauthCalls=0,apiCalls=0;
 const mockFetch=async(url,init)=>{
  if(url==='https://oauth.onshape.com/oauth/token'){
   oauthCalls++; assert.equal(init.method,'POST');
   assert.match(init.body,/grant_type=authorization_code/);
   return new Response(JSON.stringify({access_token:'PRIVATE-TOKEN',refresh_token:'PRIVATE-REFRESH',expires_in:3600}),{status:200,headers:{'content-type':'application/json'}});
  }
  const u=new URL(url);
  assert.equal(u.origin,'https://cad.onshape.com');
  assert.match(u.pathname,/^\/api\/v9\/assemblies\/d\//);
  assert.equal(init.headers.Authorization,'Bearer PRIVATE-TOKEN');
  assert.equal(init.method,undefined);
  apiCalls++;
  return new Response(JSON.stringify(fixture),{status:200,headers:{'content-type':'application/json'}});
 };
 await withServer({palettes:palette,demo:fixture,env:{ONSHAPE_CLIENT_ID:'test-client',ONSHAPE_CLIENT_SECRET:'test-secret'},fetchImpl:mockFetch},async base=>{
  const start=await fetch(base+'/auth/start',{redirect:'manual'});
  assert.equal(start.status,302);
  const cookie=start.headers.get('set-cookie').split(';')[0];
  const authUrl=new URL(start.headers.get('location'));
  assert.equal(authUrl.hostname,'oauth.onshape.com');
  assert.equal(authUrl.searchParams.get('scope'),'OAuth2Read');
  const state=authUrl.searchParams.get('state');
  assert.ok(state);
  const invalid=await fetch(base+'/auth/callback?code=abc&state=WRONG',{headers:{Cookie:cookie}});
  assert.equal(invalid.status,403);
  const callback=await fetch(base+'/auth/callback?code=abc&state='+state,{headers:{Cookie:cookie},redirect:'manual'});
  assert.equal(callback.status,303);
  const replay=await fetch(base+'/auth/callback?code=abc&state='+state,{headers:{Cookie:cookie}});
  assert.equal(replay.status,403);
  const planRes=await fetch(base+'/api/plan?theme=MONOCHROME&url='+encodeURIComponent(doc),{headers:{Cookie:cookie}});
  assert.equal(planRes.status,200);
  const raw=await planRes.text();
  assert.ok(!raw.includes('PRIVATE-TOKEN'));
  assert.ok(!raw.includes('PRIVATE-REFRESH'));
  assert.equal(JSON.parse(raw).uniqueSourceCount,1);
  assert.equal(oauthCalls,1);assert.equal(apiCalls,1);
  const badUrl=await fetch(base+'/api/plan?url='+encodeURIComponent('https://evil.example/documents/x'),{headers:{Cookie:cookie}});
  assert.equal(badUrl.status,400);assert.equal(apiCalls,1);
  const disabled=await fetch(base+'/api/apply',{method:'POST',headers:{Cookie:cookie}});
  assert.equal(disabled.status,501);
 });
});
