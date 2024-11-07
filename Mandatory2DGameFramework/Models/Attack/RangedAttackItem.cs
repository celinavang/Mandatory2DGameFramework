using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.models.attack
{
    public class RangedAttackItem : IAttackItem
    {
        public string Name { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public bool Lootable { get; set; }
        public bool Removeable { get; set; }
        public DamageType DamageType { get; set; }
        public AttackType AttackType { get; set; }
        public int MaxDamage { get; set; }

        public RangedAttackItem(string name, int maxDamage)
        {
            Name = name;
            Lootable = true;
            Removeable = true;
            DamageType = DamageType.dexterity;
            AttackType = AttackType.melee;
            MaxDamage = maxDamage;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
