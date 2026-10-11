# Stickyfoam Continued

A RimWorld 1.6 continuation of [katana's Stickyfoam](https://github.com/katanacrimson/rimworld-stickyfoam), adding mobility-restricting foam through mortar shells and IED traps. Original author credits and the MIT license are retained.

## Replacing the original

1. Back up your save. Close RimWorld before changing mod files.
2. Disable the original Stickyfoam. Install this continuation and enable it after Harmony.
3. Load a disposable copy of your save first and check coated pawns, ground foam, shells and traps.

The continuation deliberately retains `katana.stickyfoam`, existing DefNames and serialized C# types. Both versions share a package ID; install and enable only one copy to avoid ambiguous mod discovery. Do not rely on load order to choose between duplicate copies.

The original Workshop item's ID is not included in this continuation. A new Workshop item must receive its own ID when publication is separately authorized.

A disposable save created by the unmodified original has passed replacement with this continuation, then save/reload, on the tested RimWorld 1.6 runtime. See the [illustrated compatibility report](docs/COMPATIBILITY_CHECKS.md) for evidence and limitations. Arbitrary existing colonies and safe removal from an ongoing colony remain untested; unchanged identifiers alone do not prove either. Combat Extended support is outside this port's scope.

## Settings

Open **Options → Mod settings → Stickyfoam Continued**. Settings apply globally across colonies when the window closes. Damage reduction is **off by default**, making coated targets easier to kill than under the original 25% protection. **Reset to defaults** retains the original movement, accuracy and ground effects with protection off; **Original balance** restores the 25% protection too.

Movement and protection affect existing coatings immediately. Ground beauty, cleanliness and cleaning work also update existing foam. Lifetime changes affect new coatings, fresh hits and newly spawned ground foam; existing timers keep their remaining time. Ground lifetime varies by ±25% around the chosen average. Pawn removal takes 10 seconds of game time by default, independently of ground cleaning work; a job retains the duration it started with.

## Building

With a .NET SDK installed, run:

```powershell
dotnet build Source/1.6/Stickyfoam.csproj -c Release -o 1.6/Assemblies
```

Package only `Stickyfoam.dll` from the build output. Harmony is provided by the required Harmony mod; do not package game references, build intermediates or local development instructions. See `docs/COMPATIBILITY_CHECKS.md` for recorded verification and remaining checks.
