import { test } from 'node:test';
import assert from 'node:assert/strict';
import { readFileSync } from 'node:fs';
import { parseAssemblyLocator, flattenAssembly, planAssembly, resolveOccurrence } from '../lib/planner.mjs';
const fixture = JSON.parse(readFileSync(new URL('../fixtures/assembly.json', import.meta.url), 'utf8'));
const palettes = { MONOCHROME: { name:'Monochrome', colors: Array.from({length:12},(_,i)=>`#${String(i).padStart(6,'0')}`) } };

test('parse legitimate Onshape URL and reject SSRF-style hosts', () => {
  const url = 'https://cad.onshape.com/documents/aaaaaaaaaaaaaaaaaaaaaaaa/w/bbbbbbbbbbbbbbbbbbbbbbbb/e/cccccccccccccccccccccccc?foo=bar';
  assert.deepEqual(parseAssemblyLocator(url), { did:'aaaaaaaaaaaaaaaaaaaaaaaa',wvm:'w',wvmid:'bbbbbbbbbbbbbbbbbbbbbbbb',eid:'cccccccccccccccccccccccc' });
  for (const bad of ['http://cad.onshape.com/documents/x','https://cad.onshape.com.evil.org/documents/x','https://cad.onshape.com@evil.org/documents/x','https://cad.onshape.com:4444/documents/x']) assert.throws(() => parseAssemblyLocator(bad));
});

test('flatten nested sources, group repeated parts, exclude suppressed and standard content', () => {
  const result = flattenAssembly(fixture);
  assert.equal(result.rows.length, 4);
  assert.equal(result.skipped.length, 3);
  assert.equal(resolveOccurrence(fixture,['motor-module','motor']).instance.partId,'m1');
  const plan = planAssembly(fixture,palettes);
  assert.equal(plan.mode,'DRY_RUN_ONLY');
  assert.equal(plan.uniqueSourceCount,3);
  assert.equal(plan.occurrenceCount,4);
  assert.equal(plan.groups.filter(x=>x.sourceShared).length,1);
  assert.equal(plan.groups.find(x=>x.partId==='frame').occurrences.length,2);
});

test('deterministic 4 permutations and 8 variations', () => {
  for (const arrangement of ['GRADIENT','ALTERNATE','SPREAD','REVERSE']) {
    for (let variation=0; variation<8; variation++) {
      const p=planAssembly(fixture,palettes,{theme:'MONOCHROME',arrangement,variation});
      assert.deepEqual(p,planAssembly(fixture,palettes,{theme:'MONOCHROME',arrangement,variation}));
      assert.equal(new Set(p.groups.map(g=>g.color)).size,3);
    }
  }
});

test('invalid data fail closed', () => {
  assert.throws(()=>flattenAssembly({rootAssembly:{}}),/occurrences/);
  assert.throws(()=>planAssembly(fixture,palettes,{theme:'UNKNOWN'}));
  assert.throws(()=>planAssembly(fixture,palettes,{variation:200}));
  assert.throws(()=>planAssembly(fixture,palettes,{arrangement:'RANDOM'}));
});

test('same child id in two subassemblies does not resolve through global ID index', () => {
  const data=structuredClone(fixture);
  data.rootAssembly.instances.push({id:'other',type:'Assembly',documentId:'444444444444444444444444',elementId:'444444444444444444444445'});
  data.subAssemblies.push({documentId:'444444444444444444444444',elementId:'444444444444444444444445',instances:[{id:'motor',type:'Part',name:'Other Motor',documentId:'555555555555555555555555',elementId:'555555555555555555555556',partId:'m2'}]});
  data.occurrences.push({path:['other','motor']});
  assert.equal(resolveOccurrence(data,['other','motor']).instance.partId,'m2');
});

test('conflicting nested source microversion is not guessed', () => {
  const d=structuredClone(fixture);
  d.subAssemblies[0].documentMicroversion='DIFFERENT';
  assert.equal(resolveOccurrence(d,['motor-module','motor']),null);
  assert.ok(flattenAssembly(d).skipped.some(x=>x.reason==='Unresolved occurrence hierarchy'));
});
