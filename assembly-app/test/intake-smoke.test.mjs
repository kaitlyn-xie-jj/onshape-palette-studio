import {test} from 'node:test';
import assert from 'node:assert/strict';
import {readFileSync} from 'node:fs';
import {parseAssemblyLocator,planAssembly} from '../lib/planner.mjs';
import {INTAKE_URL,assemblyApiUrl,smoke} from '../tools/intake-smoke.mjs';

const fixturePath = new URL('../fixtures/assembly.json', import.meta.url).pathname;
const fixture=JSON.parse(readFileSync(fixturePath,'utf8'));
const palettes=JSON.parse(readFileSync(new URL('../../palettes.json',import.meta.url),'utf8'));

test('FRC 6381 Intake link has well-formed Onshape assembly locator',()=>{
  const l=parseAssemblyLocator(INTAKE_URL);
  assert.deepEqual(l,{did:'3d213b46f3cbe0469ea8c4ec',wvm:'w',wvmid:'1b16fe644575203f9b5724c9',eid:'a4889c26a685a3b57089aaa5'});
  const url=assemblyApiUrl(INTAKE_URL);
  assert.equal(url.hostname,'cad.onshape.com');
  assert.equal(url.pathname,`/api/v9/assemblies/d/${l.did}/w/${l.wvmid}/e/${l.eid}`);
});

test('without OAuth token the live Intake test is blocked before any fetch',async()=>{
  let calls=0;
  const r=await smoke({fetchImpl:async()=>{calls++;throw Error('network should not be used');}});
  assert.equal(r.status,'BLOCKED_AUTH');
  assert.equal(calls,0);
});

test('30 palettes x 4 arrangements x 8 variations are deterministic on SYNTHETIC source fixture',()=>{
  assert.equal(Object.keys(palettes).length,30);
  for(const theme of Object.keys(palettes))
    for(const arrangement of ['GRADIENT','ALTERNATE','SPREAD','REVERSE'])
      for(let variation=0;variation<8;variation++) {
        const p=planAssembly(fixture,palettes,{theme,arrangement,variation});
        assert.equal(p.mode,'DRY_RUN_ONLY');
        assert.equal(p.uniqueSourceCount,3);
        assert.equal(p.occurrenceCount,4);
        assert.equal(p.skipped.length,3);
        assert.equal(p.groups.find(g=>g.partId==='frame').occurrences.length,2);
        assert.equal(new Set(p.groups.map(g=>g.color)).size,3);
      }
});

test('mock authenticated GET carries bearer to fixed Onshape API and never writes',async()=>{
  let methods=[];
  const r=await smoke({accessToken:'IN_MEMORY_TEST_TOKEN',fetchImpl:async(url,options)=>{
    methods.push(options.method);
    assert.equal(new URL(url).origin,'https://cad.onshape.com');
    assert.equal(options.headers.Authorization,'Bearer IN_MEMORY_TEST_TOKEN');
    assert.equal(options.redirect,'manual');
    return new Response(JSON.stringify(fixture),{status:200,headers:{'content-type':'application/json'}});
  }});
  assert.equal(r.status,'PLAN_GENERATED');
  assert.equal(r.dataOrigin,'ONSHAPE_AUTHENTICATED_API');
  assert.equal(r.appearanceWrites,0);
  assert.deepEqual(methods,['GET']);
  assert.equal(JSON.stringify(r).includes('IN_MEMORY_TEST_TOKEN'),false);
});

test('mock permission denial fails closed and does not fabricate model counts',async()=>{
  const r=await smoke({accessToken:'MOCK',fetchImpl:async()=>new Response('not authorized',{status:403})});
  assert.equal(r.status,'BLOCKED_API');
  assert.equal(r.httpStatus,403);
  assert.equal(r.groups,undefined);
});