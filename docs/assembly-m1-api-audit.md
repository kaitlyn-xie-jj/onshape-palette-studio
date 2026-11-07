# Assembly Palette Studio M1 — API audit (7 Nov 2026)

## Verified from Onshape developer documentation

- The **getAssemblyDefinition** endpoint is documented as `GET /api/v9/assemblies/d/{did}/{wvm}/{wvmid}/e/{eid}` with optional query flags. Returned objects include a `rootAssembly`, subassembly instances and occurrence paths. [Assemblies API](https://onshape-public.github.io/docs/api-adv/assemblies/).
- Appearance can be assigned in a **Part Studio** and is normally displayed in Assemblies referencing that part. [Appearance editor](https://cad.onshape.com/help/Content/PartStudio/customizing_appearances.htm).
- Onshape in-app extensions can embed an **HTTPS iframe** and pass current document/element IDs through substituted URL parameters. [Extensions](https://onshape-public.github.io/docs/app-dev/extensions/).
- OAuth2 is the supported path for app authorization, `OAuth2Read` is the read-documents scope. [OAuth2](https://onshape-public.github.io/docs/auth/oauth/), [Developer settings](https://cad.onshape.com/help/Content/Plans/my_account_developer.htm).

## Important gap / no invented capabilities

The documented Assembly `POST .../modify` operations address transforms, suppression etc., not a generic instance-appearance mutation. A stable documented REST API for arbitrary instance-specific RGB updates has **not** been established. This milestone therefore stops after verified source mapping and review/export planning. There is no hidden simulated "success" or direct source-file write.

**No live Onshape OAuth login or FeatureScript compilation has been tested** because this repository alone does not have authorized Onshape credentials, an installed extension or a disposable test document.

## Acceptance for M2 write bridge

1. Use a disposable Onshape document and test a documented/permissioned appearance mutation route, recording exact request and resulting appearance state.
2. Verify source part identity, configuration, workspace/version mutability, released parts and edit permissions. Distinguish per-source color from per-instance override.
3. Stage a dry-run diff, request explicit user confirmation, then apply only to chosen eligible sources; record before/after evidence and detect partial failure.
4. Regression tests for repeated parts, deeply nested subassemblies, standard content, mixed fixed/linked versions, missing permission, conflicting requirements and rollback.
5. Real Onshape FeatureScript compile and Assembly UI test before advertising "one-click apply".
