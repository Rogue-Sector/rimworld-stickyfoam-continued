# Stickyfoam Continued

A RimWorld 1.6 continuation of [katana's Stickyfoam](https://github.com/katanacrimson/rimworld-stickyfoam), adding mobility-restricting foam through mortar shells and IED traps. Original author credits and the MIT license are retained.

**1.6 beta candidate.** Testing is performed by Codex in disposable colonies; no human playtesting is claimed or required as a release gate. Uninstall from ongoing saves is unsupported, genuine 1.5 migration is unverified, and arbitrary third-party mod combinations are untested. North-facing dusters can hide the inherited foam overlay. Publication remains the owner's decision.

**Current checkpoint:** final beta validation is paused. Read the [current validation status](docs/VALIDATION_STATUS.md) before using the historical reports or PDF as release evidence.

## Replacing the original

1. Back up your save. Close RimWorld before changing mod files.
2. Disable the original Stickyfoam. Install this continuation and enable it after Harmony.
3. Load a disposable copy of your save first and check coated pawns, ground foam, shells and traps.

The continuation deliberately retains `katana.stickyfoam`, existing DefNames and serialized C# types. Both versions share a package ID; install and enable only one copy to avoid ambiguous mod discovery. Do not rely on load order to choose between duplicate copies.

The original Workshop item's ID is not included in this continuation. A new Workshop item must receive its own ID when publication is separately authorized.

A disposable save created by the unmodified original has passed replacement with this continuation, then save/reload, on the tested RimWorld 1.6 runtime. See the [illustrated compatibility report](docs/COMPATIBILITY_CHECKS.md) for evidence and limitations. Genuine 1.5-to-1.6 migration is blocked by unavailable historical prerequisites; arbitrary existing colonies remain untested. **Removing the mod from an ongoing save containing foam or removal jobs is unsafe and unsupported.** A test-assisted cleaned fixture passed removal, but it does not establish a normal user preparation workflow. Combat Extended support is outside this port's scope.

## Settings

Open **Options → Mod settings → Stickyfoam Continued**. Settings apply globally across colonies when the window closes. Damage reduction is **off by default**, making coated targets easier to kill than under the original 25% protection. **Reset to defaults** retains the original movement, accuracy and ground effects with protection off; **Original balance** restores the 25% protection too.

Movement and protection affect existing coatings immediately. Ground beauty, cleanliness and cleaning work also update existing foam. Lifetime changes affect new coatings, fresh hits and newly spawned ground foam; existing timers keep their remaining time. Ground lifetime varies by ±25% around the chosen average. Pawn removal takes 10 seconds of game time by default, independently of ground cleaning work; a job retains the duration it started with.

## Removing coating

Select an eligible coated colonist and right-click that pawn for **Remove foam**. This timed job is available to awake, undrafted, capable player-controlled humanlike colonists. You may deliberately order it during a raid; drafting, incapacitation or external harmful damage interrupts it.

Colonists also remove their coating during safe idle time, after normal work and needs, even when Cleaning is disabled. Automatic removal waits during active hostile threats, local danger, combat, fleeing, rescue, caravan formation or assigned duties. A threat beginning during removal interrupts the automatic job; its driver rechecks threats every 60 ticks using the game's attack-target cache. Ordering **Remove foam** during an existing automatic job turns it into a manual override without resetting progress.

Ordinary interruption allows another automatic attempt after 600 ticks when safe. Explicitly cancelling with another order suppresses automatic removal for that coating until a fresh hit or a new manual removal order. This state survives save/reload. Animals, mechs, prisoners and downed pawns retain natural expiry. Removing a pawn's coating leaves ground foam in place and does not cause walk-over recoating.

See the [illustrated settings and removal report](docs/SETTINGS_AND_REMOVAL_CHECKS.md) for settings ranges, actual game checks and remaining work.

The [release-candidate report](docs/RELEASE_CANDIDATE_CHECKS.md) records the official-DLC, persisted-colony, caravan, visual, uninstall and maintenance checks, exact candidate hashes, agent-run gameplay scope and remaining limitations. Its pictures include ground foam, coated pawns, apparel and downed-pawn portraits. Supporting evidence documents are bundled with the report. Public release remains a separate owner decision.

## Building

With a .NET SDK installed, run:

```powershell
dotnet build Source/1.6/Stickyfoam.csproj -c Release -o 1.6/Assemblies
```

Package only `Stickyfoam.dll` from the build output. Harmony is provided by the required Harmony mod; do not package game references, build intermediates or local development instructions. See `docs/COMPATIBILITY_CHECKS.md` for recorded verification and remaining checks.
