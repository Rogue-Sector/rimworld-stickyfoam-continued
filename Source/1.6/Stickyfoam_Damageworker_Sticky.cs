using RimWorld;
using Verse;

namespace Stickyfoam
{
    public class DamageWorker_Sticky : DamageWorker
    {
        public override DamageResult Apply(DamageInfo dinfo, Thing victim)
        {
            DamageResult result = new DamageResult();
            if (victim is Pawn pawn)
            {
                Hediff hediff = HediffMaker.MakeHediff(dinfo.Def.hediff, pawn);
                hediff.Severity = dinfo.Amount;
                pawn.health.AddHediff(hediff, null, dinfo);
            }
            return result;
        }
    }
}
