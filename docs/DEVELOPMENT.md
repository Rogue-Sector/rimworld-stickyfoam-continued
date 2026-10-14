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

Tested by Codex in disposable colonies (no human playtest) on RimWorld 1.6.4871: Core and all five official DLC, Combat Extended 16.7.3, Vanilla Expanded animal packs, Alpha Animals and Humanoid Alien Races (Revia). Detailed development reports and screenshots are retained in the local backup, rather than in this repository.

When testing locally, never touch the real game setup:

- Launch with `-savedatafolder=PATH` (no space after `=`) pointing at a throwaway folder, and confirm it before the game starts.
- Hold the read-only guard on the real `Config` files for every launch, including the first.
- Use a throwaway mods folder with only Harmony and the built package. Never modify the real Mods folder, saves or Workshop subscriptions.
- The disposable setup and launch helper live under `%LOCALAPPDATA%\stickyfoam-dev\` on the development PC; screenshots go in the gitignored `playtest-screenshots/` folder.
