## Stickyfoam changelog

#### v2.0.0-beta1 (Stickyfoam Continued)

First release of the continuation by BiggestFren. Replaces the original; do not enable both.

- Ported to RimWorld 1.6 (built against krafs.rimworld.ref 1.6.4871 and Harmony 2.4.1). RimWorld 1.5 is no longer supported.
- Added a stickyfoam turret: mini-turret build cost, Stickyfoam and Gun turrets research, 20-tile range, two-tile foam burst and one shot about every five seconds. Fires with allies near the target; friendly fire can coat them.
- Added machining-table stickyfoam canisters costing 10 steel. Colonists rearm turrets with 10 shots per canister, up to 30 shots; empty turrets stop firing.
- Replaced pink foam with gold (#E7B921): glossy ground puddles and pawn glaze, matching shells, projectiles, IEDs and the turret's rotating rear capsule. Vanilla turret textures remain untinted.
- Added a mod settings menu: damage reduction, ground foam beauty, cleanliness, cleaning work and lifetime, pawn slowdown, coating duration and removal time. Includes **Reset to defaults** and **Original balance**.
- **Balance change:** the coating's 25% damage reduction is now off by default, so coated targets are easier to kill. **Original balance** turns it back on.
- Added **Remove foam**: right-click a coated colonist to have them scrape the foam off. This is a timed job you can order even during a raid.
- Colonists now remove their own coating during safe idle time, without needing the Cleaning work type. They won't stop to do it during raids, combat, fleeing, rescues or caravan forming.
- Fixed coating overlay data being kept in memory for pawns after their coating ended or they were destroyed.
- Removed the original mod's Workshop ID from the package.
- Known limitations: removing the mod from an ongoing save is unsupported; migrating saves from RimWorld 1.5 is untested; Combat Extended is unsupported; Eccentric Tech Defense Grid integration is untested on 1.6; north-facing dusters can hide the foam overlay.


#### v1.0.0-beta4

- Rebuilt against updated krafs.rimworld.ref (1.5.4051)
- Rebuilt against updated harmony (2.3.3)
- Minor csproj cleanup
- Now packaging LICENSE file

#### v1.0.0-beta3

- Now display stickyfoam overlay on non-humanlikes (animals, mechs)
- Rebuilt against latest krafs.rimworld.ref
- Added support for Eccentric Tech Defense Grid - now includes the M43 Stickyfoam PGM

#### v1.0.0-beta2

- Internal beta, not released due to bugs discovered after tagging.

#### v1.0.0-beta1

- rimworld 1.5 support
