using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.models.attack
{
    /*!
     * \class MeleeAttackItem
     * \brief Represents a melee attack item (e.g., a weapon) in the game world.
     */
    public class MeleeAttackItem : IAttackItem
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
 * \brief Constructor for a melee attack item.
 * \param name Item name.
 * \param maxDamage Max damage this item can deal.
 */
        public MeleeAttackItem(string name, int maxDamage)
        {
            Name = name;
            Lootable = true;
            Removeable = true;
            DamageType = DamageType.strength;
            AttackType = AttackType.melee;
            MaxDamage = maxDamage;
        }

        /*!
         * \brief Returns a string representation of the melee attack item.
         */
        public override string ToString()
        {
            return $"Melee Attack Item {Name} | Max damage: {MaxDamage}";
        }
    }
}

