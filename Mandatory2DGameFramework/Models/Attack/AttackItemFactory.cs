using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.models.attack
{
    public class AttackItemFactory
    {
        public static IAttackItem CreateAttackItem(AttackType attackType, string name, int maxDamage)
        {
            switch (attackType)
            {
                case AttackType.melee:
                    Trace.TraceInformation("Creating melee attack item:");
                    Trace.Flush();
                    return new MeleeAttackItem(name, maxDamage);
                case AttackType.ranged:
                    Trace.TraceInformation("Creating ranged attack item");
                    Trace.Flush();
                    return new RangedAttackItem(name, maxDamage);
                default:
                    Trace.TraceError("Could not create Attack Item - Invalid attack type");
                    throw new Exception("Invalid attack type");
            }

        }
    }
}
