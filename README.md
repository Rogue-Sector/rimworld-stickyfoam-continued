# Stickyfoam Continued

A RimWorld 1.6 continuation of [katana's Stickyfoam](https://github.com/katanacrimson/rimworld-stickyfoam). Stickyfoam mortar shells, IED traps and turrets coat targets in foam that slows them down and throws off their aim. Gold glossy puddles cover the ground, and a matching glossy glaze coats pawns.

**Beta.** Original author credits and the MIT license are retained. Requires [Harmony](https://steamcommunity.com/sharedfiles/filedetails/?id=2009463077).

## Replacing the original

This mod replaces the original Stickyfoam and uses the same package ID, so **enable only one of them**.

1. Back up your save.
2. Disable or unsubscribe from the original Stickyfoam.
3. Enable Stickyfoam Continued after Harmony.

Existing foam, shells and traps carry over.

## Stickyfoam turret

A 1×1 automatic turret with the vanilla mini-turret's build cost. Requires **Stickyfoam** and **Gun turrets** research. It fires one foam shot about every five seconds, with a 20-tile range and a two-tile burst radius. Friendly fire is enabled: nearby colonists can be coated too.

Make **stickyfoam canisters** at a machining table after Stickyfoam research, for **10 steel** each. Colonists haul them to rearm the turret. Each canister adds **10 shots**, up to **30**; an empty turret stops firing. The untinted vanilla turret has a gold-banded rear capsule that turns with its gun.

## Combat Extended

Combat Extended is supported for **mortar shells, stickyfoam IEDs and the stickyfoam turret**. Load Stickyfoam Continued after Combat Extended. Craft stickyfoam shells at the machining table after **Stickyfoam** research, for **25 steel** each, and select **stickyfoam** ammunition on a CE 81mm mortar. Shells coat targets and leave gold ground foam. The turret continues to use stickyfoam canisters.

## Settings

**Options → Mod settings → Stickyfoam Continued.** Settings are global and apply when you close the window.

- **Damage reduction** is **off by default**, so coated targets are easier to kill than in the original. Turn it on, or press **Original balance**, to restore the original 25% reduction.
- **Ground foam:** beauty, cleanliness, cleaning work and how long it lasts.
- **Pawn coating:** movement slowdown, how long it lasts and how long removal takes (10 seconds of game time by default).
- **Reset to defaults** restores everything with damage reduction off.

Slowdown, damage reduction, beauty, cleanliness and cleaning work change existing foam right away. Duration changes apply to new foam.

## Removing foam

- **Manually:** right-click a coated colonist and choose **Remove foam**. It takes a short time and works even during a raid. Drafting, getting downed or taking damage interrupts it.
- **Automatically:** colonists scrape off their own foam when they're idle and it's safe, even with Cleaning disabled. They won't stop to do it during raids, combat, fleeing or rescues.
- Animals, mechs, prisoners and downed pawns can't remove foam; it wears off on its own.
- Removing a pawn's coating doesn't clean the floor. Ground foam is cleaned like normal filth and washes away in rain.

## Known limitations

- **Don't remove this mod from an ongoing save** that has used it. Keep it installed, or go back to a save from before you added it.
- Migrating a save straight from RimWorld 1.5 is untested.
- Eccentric Tech Defense Grid integration (the M43 Stickyfoam PGM) is untested on 1.6.
- Humanoid Alien Races: foam coats the body, but race add-ons such as ears and tails stay uncoated. Coating and Remove foam still work.
- Tested with the base game and all official DLC; separately with Combat Extended; and with Vanilla Expanded animal packs, Alpha Animals and Humanoid Alien Races (Revia), where coating fits modded animal shapes. Other mod combinations are untested.

## Bugs and feedback

Please report bugs with your `Player.log` and your mod list.
