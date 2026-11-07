# Assembly Palette Studio — M1 read-only prototype

This is the next development slice of [Onshape Palette Studio](../README.md). It converts a *real Assembly definition* into a cross-Part-Studio color plan, reusing the existing **30 themes** in `../palettes.json`. Nothing in this app modifies CAD yet.

## What works

- Local server-side OAuth2 authorization flow using **OAuth2Read only**; tokens remain in an ephemeral in-memory session (never localStorage or the browser UI). OAuth state is single-use and cookie-bound. Refresh is supported.
- Read a selected Onshape Assembly via `GET /api/v9/assemblies/d/{did}/{wvm}/{wvmid}/e/{eid}` (read-only).
- Resolve nested occurrence paths to source part IDs; skip suppressed / standard content / unresolved paths instead of fabricating colors.
- Group repeated occurrences by **source part + configuration + microversion**; generate deterministic 30-theme palette assignments and preview them.
- Export a JSON or CSV **review plan**, including skipped records and safety warnings.
- Local offline demo and automated tests without any account, secrets, or API calls.

## Run locally

Requires Node.js 20+. No npm dependencies.

```bash
cd assembly-app
node --test test/*.test.mjs
# First configure OAuth in Onshape Developer settings (read scope only) and export credentials:
export ONSHAPE_CLIENT_ID='...'
export ONSHAPE_CLIENT_SECRET='...'
export PUBLIC_ORIGIN='http://127.0.0.1:8787'
node server.mjs
```

Open `http://127.0.0.1:8787/`. Select **示例装配体预览** before connecting an account. To read a real Assembly, register a private Onshape OAuth application (permission **OAuth2Read** only) with callback `http://127.0.0.1:8787/auth/callback`, then use **连接 Onshape**. The document URL must contain a real Assembly tab. Copying a document URL with a Part Studio tab cannot work. Free plans may have additional API and public document limitations.

**Do not commit API keys, OAuth tokens or Client Secrets.** The OAuth app must be separately registered in your own Onshape Developer settings; this code does not grant itself permissions.

## In-Onshape panel integration (later, after HTTPS deployment)

Onshape supports an extension panel via an HTTPS iframe. Once an HTTPS host and OAuth callback are deployed, an extension URL can be configured using these Onshape substitutions:

```text
https://YOUR_DEPLOYED_HOST/?documentId={$documentId}&workspaceOrVersion={$workspaceOrVersion}&workspaceOrVersionId={$workspaceOrVersionId}&tabElementId={$tabElementId}
```

This prototype deliberately limits source URLs to `cad.onshape.com` (no arbitrary server-URL proxy). Local HTTP development uses a Lax cookie; deployed HTTPS uses `Secure; SameSite=None` for iframe sessions. **HTTPS deployment, installing the extension, testing OAuth and actual Onshape Feature Studio compilation are not yet done.** Before public deployment, replace the process-local session store with an encrypted, expiring session store and implement multi-instance/production hardening.

## Limitation: what "one click" can and cannot mean here

The public `GET assembly definition` endpoint provides occurrence and source IDs. Published Assembly `/modify` examples do **not** expose a supported arbitrary part/instance appearance write. This phase is therefore *planning, not execution*. The `Apply` control is intentionally disabled and `/api/apply` responds HTTP 501 rather than pretending a write succeeded. A later adapter can run only after confirming a supported write mechanism in a disposable Onshape document, checking source edit permissions/version links and reversible operations. A source Part Studio appearance propagates to other assemblies that use that part; repeated instances cannot be recolored individually by modifying one source part.

## Security / quality gates

- Read-only OAuth scope; no model mutation endpoints, no destructive/hidden fallback.
- Strict `https://cad.onshape.com/documents/...` URL validation and fixed API host; no arbitrary host forwarding or SSRF.
- No secret logging, browser token exposure, or unverified OAuth claims.
- Fail closed on missing/malformed `occurrences`; skipped unresolved and suppressed instances are visible.
- Deterministic output, source grouping, and CSV/JSON export.

## Official references

- [Onshape Assemblies API](https://onshape-public.github.io/docs/api-adv/assemblies/)
- [Onshape OAuth2](https://onshape-public.github.io/docs/auth/oauth/)
- [Onshape app extensions](https://onshape-public.github.io/docs/app-dev/extensions/)
- [Appearance editor](https://cad.onshape.com/help/Content/PartStudio/customizing_appearances.htm)

## FRC 6381 Intake smoke test (2026-11-07)

A published FRC Intake document is now the first concrete M2 test target. See [the test record](../docs/intake-smoke-test-2026-11-07.md). This target is **not yet confirmed through a live Onshape API read** because an authorized OAuth session is required. Offline test counts are from a deliberately labelled synthetic fixture.

```bash
# At repository root:
node --test assembly-app/test/*.test.mjs
node assembly-app/tools/intake-smoke.mjs --fixture assembly-app/fixtures/assembly.json --theme MONOCHROME
node assembly-app/tools/intake-smoke.mjs  # fails closed with BLOCKED_AUTH until OAuth is configured
```

The CLI also supports `--live` using a private `ONSHAPE_ACCESS_TOKEN`, but the existing OAuth-enabled web UI is preferable for normal users. Neither mode modifies source parts or assembly instances.