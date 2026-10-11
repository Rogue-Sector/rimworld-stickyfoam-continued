# Development notes

## Building

With a .NET SDK installed, run:

```powershell
dotnet build Source/1.6/Stickyfoam.csproj -c Release -o 1.6/Assemblies
```

Package only `Stickyfoam.dll` from the build output. Harmony is provided by the required Harmony mod. Do not package game reference assemblies, build intermediates or local development instructions.

The release package contains `About/`, `1.6/` (with `Assemblies/Stickyfoam.dll`), `Textures/`, `ModPatches/`, `LoadFolders.xml`, `LICENSE` and `CHANGELOG.md`.

## Identity decisions

- The continuation deliberately keeps the `katana.stickyfoam` package ID, existing DefNames and serialized C# types so existing saves keep working. Because both versions share a package ID, only one copy may be installed and enabled; load order cannot choose between duplicates.
- The original Workshop item's ID is not included. A new Workshop item gets its own ID when published.

## Testing

Testing was done by Codex in disposable colonies; no human playtesting was done. Records:

- [Validation status](VALIDATION_STATUS.md): current state and what is out of scope.
- [Compatibility checks](COMPATIBILITY_CHECKS.md): the 1.6 port baseline, including replacing the original mod in a 1.6 save.
- [Settings and removal checks](SETTINGS_AND_REMOVAL_CHECKS.md): settings ranges and removal behaviour.
- [Release candidate checks](RELEASE_CANDIDATE_CHECKS.md): DLC, caravan, visual, uninstall and maintenance runs.
- [Release candidate report (PDF)](../output/pdf/Stickyfoam-Continued-Release-Candidate-Report.pdf).
