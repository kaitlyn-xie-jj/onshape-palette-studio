/**
 * Read-only Onshape Assembly integration smoke test.
 * A fixture run is NOT evidence of reading the real CAD model.
 * Real API mode requires ONSHAPE_ACCESS_TOKEN from your private environment.
 */
import { readFileSync, writeFileSync } from 'node:fs';
import { fileURLToPath } from 'node:url';
import { resolve } from 'node:path';
import { parseAssemblyLocator, planAssembly } from '../lib/planner.mjs';

export const INTAKE_URL = 'https://cad.onshape.com/documents/3d213b46f3cbe0469ea8c4ec/w/1b16fe644575203f9b5724c9/e/a4889c26a685a3b57089aaa5';

export function assemblyApiUrl(locator) {
  const l = parseAssemblyLocator(locator);
  const url = new URL(`https://cad.onshape.com/api/v9/assemblies/d/${l.did}/${l.wvm}/${l.wvmid}/e/${l.eid}`);
  for (const [key,value] of Object.entries({
    includeMateFeatures:'false',includeNonSolids:'false',includeMateConnectors:'false',excludeSuppressed:'false'
  })) url.searchParams.set(key,value);
  return url;
}

export async function smoke({target = INTAKE_URL, fixturePath, accessToken, theme = 'MONOCHROME',fetchImpl = fetch} = {}) {
  const locator = parseAssemblyLocator(target);
  if (!fixturePath && !accessToken) {
    return { status: 'BLOCKED_AUTH', target: locator, reason:'Real assembly API requires an Onshape OAuth access token. No network request was made.' };
  }
  let definition;
  const dataOrigin = fixturePath ? 'LOCAL_FIXTURE_NOT_REAL_CAD' : 'ONSHAPE_AUTHENTICATED_API';
  if (fixturePath) {
    definition = JSON.parse(readFileSync(resolve(fixturePath),'utf8'));
  } else {
    const response = await fetchImpl(assemblyApiUrl(target).toString(),{
      method: 'GET',
      headers: {Authorization:`Bearer ${accessToken}`, Accept: 'application/json'},
      redirect: 'manual', signal: AbortSignal.timeout(15000)
    });
    if (!response.ok || response.status >= 300) {
      return {status:'BLOCKED_API', target:locator, dataOrigin, httpStatus:response.status,
        reason:'Onshape Assembly definition was not returned. Check OAuth read scope and document access.'};
    }
    if (!String(response.headers.get('content-type')||'').includes('json')) {
      return {status:'BLOCKED_API',target:locator,dataOrigin,reason:'API response did not declare JSON.'};
    }
    definition = await response.json();
  }
  const palettes = JSON.parse(readFileSync(new URL('../../palettes.json',import.meta.url),'utf8'));
  const plan = planAssembly(definition, palettes, { theme, arrangement:'SPREAD', variation:0 });
  return {
    status: 'PLAN_GENERATED', dataOrigin, target:locator,
    theme: plan.theme, occurrenceCount:plan.occurrenceCount,
    uniqueSourceCount:plan.uniqueSourceCount, skippedCount:plan.skipped.length,
    groups:plan.groups.map(g=>({sourceKey:g.sourceKey,name:g.name,color:g.color,occurrences:g.occurrences.length})),
    skipped:plan.skipped, appearanceWrites:0,
    note:fixturePath ? 'Synthetic fixture only; not a test of the live FRC 6381 Intake.'
      : 'Read-only Onshape API response. Appearance write, model copy and Onshape compilation were NOT performed.'
  };
}

function usage() {
  return `Usage:\n  node assembly-app/tools/intake-smoke.mjs                         # reports missing OAuth safely\n  node assembly-app/tools/intake-smoke.mjs --fixture assembly-app/fixtures/assembly.json\n  ONSHAPE_ACCESS_TOKEN=... node assembly-app/tools/intake-smoke.mjs --live\n  Optional: --url <Onshape-assembly-url> --theme <palette-id> --output <json-file>\nNever commit, log or share your OAuth token. Use a disposable COPY for future write tests.`;
}
export async function cli(argv = process.argv.slice(2)) {
  const opts = {};
  for(let i=0;i<argv.length;i++) {
    const arg=argv[i];
    if (arg==='--help') { console.log(usage()); return 0; }
    if (arg==='--live') { opts.live=true; continue; }
    if (['--fixture','--url','--theme','--output'].includes(arg) && i+1<argv.length) {
      opts[arg.slice(2)] = argv[++i]; continue;
    }
    console.error(`Unknown/incomplete argument: ${arg}`); console.log(usage()); return 2;
  }
  if (opts.live && opts.fixture) {console.error('Choose either --live or --fixture.');return 2;}
  let result;
  try {
    result=await smoke({target:opts.url || INTAKE_URL,fixturePath:opts.fixture,
      accessToken: opts.live ? process.env.ONSHAPE_ACCESS_TOKEN : undefined,
      theme:opts.theme || 'MONOCHROME'});
  } catch(e) {console.error('FAILED_VALIDATION: ' + e.message);return 1;}
  console.log(JSON.stringify(result,null,2));
  if (opts.output) writeFileSync(resolve(opts.output),JSON.stringify(result,null,2)+'\n',{encoding:'utf8',mode:0o600});
  return result.status==='PLAN_GENERATED' ? 0 : result.status==='BLOCKED_AUTH'?3:4;
}
if (process.argv[1] && resolve(process.argv[1])===fileURLToPath(import.meta.url)) {
  process.exitCode=await cli();
}