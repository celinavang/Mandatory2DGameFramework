using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.models.attack
{
    /*!
    * \class RangedAttackItem
    * \brief Represents a ranged weapon item with dexterity-based damage.
    */
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

        /*!
        * \brief Constructor for a ranged attack item.
        * \param name Item name.
        * \param maxDamage Max damage this item can deal.
        */
        public RangedAttackItem(string name, int maxDamage)
        {
            Name = name;
            Lootable = true;
            Removeable = true;
            DamageType = DamageType.dexterity;
            AttackType = AttackType.melee;
            MaxDamage = maxDamage;
        }
        /*!
 * \brief Returns a string representation of the ranged attack item.
 */
        public override string ToString()
        {
            return Name;
        }
    }
}
