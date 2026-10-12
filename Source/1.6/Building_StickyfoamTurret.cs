using RimWorld;
using UnityEngine;
using Verse;

namespace Stickyfoam
{
    [StaticConstructorOnStartup]
    public class Building_StickyfoamTurret : Building_TurretGun
    {
        private static readonly Material Tube = MaterialPool.MatFrom(
            "Things/Building/Security/StickyfoamTurret/M1_Link", ShaderDatabase.Cutout);
        private static readonly Material Capsule = MaterialPool.MatFrom(
            "Things/Building/Security/StickyfoamTurret/M1_Tank", ShaderDatabase.Cutout);

        protected override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            base.DrawAt(drawLoc, flip);
            Quaternion rotation = (CurrentEffectiveVerb?.AimAngleOverride ?? Top.CurRotation).ToQuat();
            Vector3 scale = new Vector3(3f, 1f, 3f);
            Graphics.DrawMesh(MeshPool.plane10,
                Matrix4x4.TRS(drawLoc + Altitudes.AltIncVect * 1.2f, rotation, scale), Tube, 0);
            Graphics.DrawMesh(MeshPool.plane10,
                Matrix4x4.TRS(drawLoc + Altitudes.AltIncVect * 1.4f, rotation, scale), Capsule, 0);
        }
    }
}
