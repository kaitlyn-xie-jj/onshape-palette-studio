/** Read-only, deterministic palette planner. This module never issues CAD writes. */
const ID = /^[0-9a-f]{24}$/i;
const ARRANGEMENTS = new Set(['GRADIENT', 'ALTERNATE', 'SPREAD', 'REVERSE']);

export function parseAssemblyLocator(input) {
  const source = String(input || '').trim();
  let url;
  try { url = new URL(source); } catch { throw new Error('Paste an Onshape document URL.'); }
  if (url.protocol !== 'https:' || url.hostname !== 'cad.onshape.com' || url.port || url.username || url.password) {
    throw new Error('Only HTTPS cad.onshape.com URLs are supported in this local prototype.');
  }
  const match = url.pathname.match(/^\/documents\/([0-9a-f]{24})\/(w|v|m)\/([0-9a-f]{24})\/e\/([0-9a-f]{24})(?:\/.*)?$/i);
  if (!match) throw new Error('Expected /documents/{did}/w|v|m/{id}/e/{assembly-eid}.');
  return { did: match[1], wvm: match[2], wvmid: match[3], eid: match[4] };
}

export function validateLocator({ did, wvm, wvmid, eid }) {
  if (![did, wvmid, eid].every(s => ID.test(String(s))) || !['w','v','m'].includes(wvm)) {
    throw new Error('Invalid document, workspace/version/microversion, or element ID.');
  }
  return { did, wvm, wvmid, eid };
}

function keyForAssembly(a) {
  return [a?.documentId || '', a?.elementId || '', a?.documentMicroversion || '', a?.fullConfiguration ?? a?.configuration ?? 'default'].join('|');
}

function childAssemblyFor(instance, subAssemblies) {
  const exact = subAssemblies.find(a => keyForAssembly(a) === keyForAssembly(instance));
  if (exact) return exact;
  // If both sides expose a version/config, contradictory values must not be silently ignored.
  const sameElement = subAssemblies.filter(a => a.documentId === instance.documentId && a.elementId === instance.elementId
    && (!a.documentMicroversion || !instance.documentMicroversion || a.documentMicroversion === instance.documentMicroversion)
    && (!(a.fullConfiguration || a.configuration) || !(instance.fullConfiguration || instance.configuration)
      || (a.fullConfiguration || a.configuration) === (instance.fullConfiguration || instance.configuration))); 
  return sameElement.length === 1 ? sameElement[0] : null;
}

/** Resolves occurrence paths in their own hierarchy, never by an ambiguous global instance ID. */
export function resolveOccurrence(definition, path) {
  if (!Array.isArray(path) || !path.length || !definition?.rootAssembly) return null;
  let assembly = definition.rootAssembly;
  const subAssemblies = Array.isArray(definition.subAssemblies) ? definition.subAssemblies : [];
  let inheritedSuppressed = false;
  for (let i = 0; i < path.length; i++) {
    const matching = (assembly.instances || []).filter(x => x.id === path[i]);
    if (matching.length !== 1) return null;
    const instance = matching[0];
    inheritedSuppressed ||= !!instance.suppressed || !!instance.isSuppressed;
    if (i === path.length - 1) return { instance, inheritedSuppressed };
    if (String(instance.type).toLowerCase() !== 'assembly') return null;
    assembly = childAssemblyFor(instance, subAssemblies);
    if (!assembly) return null;
  }
  return null;
}

export function flattenAssembly(definition) {
  if (!definition?.rootAssembly) throw new Error('Assembly definition is missing rootAssembly.');
  if (!Array.isArray(definition.occurrences)) throw new Error('Assembly response has no occurrences array; refusing to invent instance paths.');
  const rows = [], skipped = [];
  for (const occurrence of definition.occurrences) {
    const path = occurrence?.path;
    const resolved = resolveOccurrence(definition, path);
    if (!resolved) {
      skipped.push({ path: Array.isArray(path) ? path : [], reason: 'Unresolved occurrence hierarchy' });
      continue;
    }
    const { instance, inheritedSuppressed } = resolved;
    if (inheritedSuppressed) { skipped.push({path, reason:'Suppressed instance'}); continue; }
    if (instance.isStandardContent) { skipped.push({path, reason:'Standard content'}); continue; }
    if (String(instance.type).toLowerCase() !== 'part') continue;
    if (!instance.documentId || !instance.elementId || !instance.partId) {
      skipped.push({path, reason:'Missing source part identity'}); continue;
    }
    const configuration = instance.fullConfiguration || instance.configuration || 'default';
    const sourceKey = [instance.documentId, instance.elementId, instance.partId, configuration, instance.documentMicroversion || ''].join('|');
    rows.push({
      path: [...path], name: String(instance.name || instance.partId), sourceKey,
      documentId: instance.documentId, elementId: instance.elementId,
      partId: instance.partId, documentMicroversion: instance.documentMicroversion || null,
      configuration,
    });
  }
  rows.sort((a,b) => a.path.join('/').localeCompare(b.path.join('/')));
  return { rows, skipped };
}

export function planAssembly(definition, palettes, options = {}) {
  const theme = String(options.theme || 'MONOCHROME');
  const entry = palettes[theme];
  if (!entry || !Array.isArray(entry.colors) || entry.colors.length !== 12) throw new Error('Unknown or malformed palette.');
  const arrangement = String(options.arrangement || 'GRADIENT');
  if (!ARRANGEMENTS.has(arrangement)) throw new Error('Invalid palette arrangement.');
  const variation = Number(options.variation ?? 0);
  if (!Number.isInteger(variation) || variation < 0 || variation > 7) throw new Error('Variation must be 0–7.');
  const { rows, skipped } = flattenAssembly(definition);
  const bySource = new Map();
  for (const row of rows) {
    const existing = bySource.get(row.sourceKey);
    if (existing) { existing.occurrences.push({ path: row.path, name: row.name }); continue; }
    bySource.set(row.sourceKey, { ...row, occurrences: [{ path: row.path, name: row.name }] });
  }
  const groups = [...bySource.values()].sort((a,b) => a.sourceKey.localeCompare(b.sourceKey));
  const offsets = [0,1,3,5,7,9,10,11];
  const offset = offsets[variation];
  for (let i = 0; i < groups.length; i++) {
    const len = entry.colors.length;
    let index;
    if (arrangement === 'ALTERNATE') index = (i*5+offset)%len;
    else if (arrangement === 'SPREAD') index = (i*7+offset)%len;
    else if (arrangement === 'REVERSE') index = len-1-((i+offset)%len);
    else index = (i+offset)%len;
    groups[i].color = entry.colors[index];
    groups[i].sourceShared = groups[i].occurrences.length > 1;
  }
  return {
    mode: 'DRY_RUN_ONLY',
    theme, themeName: entry.name, arrangement, variation,
    occurrenceCount: rows.length,
    uniqueSourceCount: groups.length,
    groups,
    skipped,
    warnings: [
      'No Onshape appearance writes are performed. This is a review/export plan.',
      'Appearance is sourced from Part Studios; changing a source part can affect other assemblies.',
      'Linked version/microversion references and permissions must be checked before any future source edit.',
      'Multiple occurrences of the same source part are assigned the same color. Assignment is not instance-specific.'
    ]
  };
}
