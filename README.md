# Stickyfoam Continued

A RimWorld 1.6 continuation of [katana's Stickyfoam](https://github.com/katanacrimson/rimworld-stickyfoam), adding mobility-restricting foam through mortar shells and IED traps. Original author credits and the MIT license are retained.

## Replacing the original

1. Back up your save. Close RimWorld before changing mod files.
2. Disable the original Stickyfoam. Install this continuation and enable it after Harmony.
3. Load a disposable copy of your save first and check coated pawns, ground foam, shells and traps.

The continuation deliberately retains `katana.stickyfoam`, existing DefNames and serialized C# types. Both versions share a package ID; install and enable only one copy to avoid ambiguous mod discovery. Do not rely on load order to choose between duplicate copies.

The original Workshop item's ID is not included in this continuation. A new Workshop item must receive its own ID when publication is separately authorized.

Save compatibility and safe removal from an ongoing colony require testing; unchanged identifiers alone do not prove either. Combat Extended support is outside this port's scope.

## Building

With a .NET SDK installed, run:

```powershell
dotnet build Source/1.6/Stickyfoam.csproj -c Release -o 1.6/Assemblies
```

Package only `Stickyfoam.dll` from the build output. Harmony is provided by the required Harmony mod; do not package game references, build intermediates or local development instructions. See `docs/COMPATIBILITY_CHECKS.md` for recorded verification and remaining checks.
