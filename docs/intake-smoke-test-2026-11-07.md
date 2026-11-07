# Intake smoke test — FRC 6381 (2026-11-07)

## Provenance and target

- Source: [Chief Delphi 2026 Open Alliance CAD directory](https://www.chiefdelphi.com/t/2026-frc-open-alliance-information-and-directory/508112/2), which explicitly labels the link `Intake` under **FRC 6381 Red Raider Robotics**.
- Onshape document link: https://cad.onshape.com/documents/3d213b46f3cbe0469ea8c4ec/w/1b16fe644575203f9b5724c9/e/a4889c26a685a3b57089aaa5
- Document ID: `3d213b46f3cbe0469ea8c4ec`; workspace ID: `1b16fe644575203f9b5724c9`; element ID: `a4889c26a685a3b57089aaa5`.
- The public link is discoverable, **but this check has not independently verified it is an Assembly tab** rather than a Part Studio, or that its authenticated REST API response is readable. Do not infer component count from the link alone.

## Actual results (test environment)

| Test | Result | Evidence scope |
| --- | --- | --- |
| Chief Delphi published CAD link exists | PASS | Source page labels link as 6381 Intake |
| Onshape locator parsing and exact API path | PASS | Local test; confirms IDs only |
| M1 test suite | PASS (13/13) | Local Node 22 run |
| 30 palettes × 4 arrangements × 8 variations | PASS (960 combinations) | **Synthetic fixture** only, not actual 6381 CAD |
| Group duplicate source parts | PASS | Synthetic nested fixture; 4 occurrences / 3 groups |
| Read-only bearer GET and no-write safeguards | PASS | Mock transport; 0 real Onshape writes |
| Live 6381 Assembly definition | BLOCKED_AUTH | No Onshape OAuth token/session available; no actual structure fetched |
| Actual part/instance appearance write | NOT TESTED | Unsupported route has not been verified; intentionally disabled |
| Onshape Feature Studio compiler and visual recoloring | NOT TESTED | Requires an editable copied Onshape document |

`assembly-app/tools/intake-smoke.mjs` returns `BLOCKED_AUTH` (exit 3) without contacting the network when a token is missing. This is an honest stop condition, **not** a failed color algorithm or a passed Onshape model integration test.

## Reproduce

Requires Node 20+ at the repo root. Never commit or paste credentials or access tokens into GitHub.

```bash
node --test assembly-app/test/*.test.mjs
node assembly-app/tools/intake-smoke.mjs  # reports BLOCKED_AUTH when missing a private token
node assembly-app/tools/intake-smoke.mjs --fixture assembly-app/fixtures/assembly.json --theme IVORY_GOLD
```

To obtain an actual read-only result, register an Onshape OAuth app with `OAuth2Read` and use the local `assembly-app/server.mjs` UI as described in [`assembly-app/README.md`](../assembly-app/README.md), then paste the FRC 6381 link there. The optional standalone CLI live probe only accepts a token from a **private environment variable**, and never writes CAD:

```bash
# Only in your own local environment after obtaining a valid OAuth2Read bearer token:
ONSHAPE_ACCESS_TOKEN='YOUR_PRIVATE_TOKEN' node assembly-app/tools/intake-smoke.mjs --live --theme IVORY_GOLD --output /tmp/intake-6381-plan.json
```

Successful live evidence requires a JSON report with `dataOrigin=ONSHAPE_AUTHENTICATED_API`, actual `occurrenceCount`, `uniqueSourceCount`, and `skippedCount`; a source-targeted screenshot or human review should follow. Do **not** identify fixture numbers as real Intake numbers.

Only after verifying a **document copy**, edit permissions and a supported appearance write API should a source modification experiment be attempted. The current read-only app never writes Onshape CAD.

## Second-stage candidate

A small gearbox can be tested next using the same harness once we have a separately verified Onshape Assembly URL and user read authorization. A real Intake serves as the initial proof gate; additional models should not obscure this unresolved live-read blocker.