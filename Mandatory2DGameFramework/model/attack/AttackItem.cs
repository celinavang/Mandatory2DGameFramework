using Mandatory2DGameFramework.worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.model.attack
{
    public enum AttackTypes { melee, ranged}
    public enum DamageTypes { dexterity, strength }
    public class AttackItem : WorldObject
    {
       

        public string  Name { get; set; }
        public int MaxHit { get; set; }
        public int Range { get; set; }
        public AttackTypes AttackType { get; set; }
        public DamageTypes DamageType { get; set; }

        public AttackItem()
        {
            Name = string.Empty;
            MaxHit = 1;
            Range = 0;
            AttackType = AttackTypes.melee;
            DamageType = DamageTypes.strength;
        }

        public int Hit()
        {
            return new Random().Next(1, MaxHit);
        }

        public override string ToString()
        {
            return $"{{{nameof(Name)}={Name}, {nameof(MaxHit)}={MaxHit.ToString()}, {nameof(Range)}={Range.ToString()}}}";
        }
    }
}
