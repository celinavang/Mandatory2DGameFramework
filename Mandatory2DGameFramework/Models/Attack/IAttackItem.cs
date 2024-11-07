using Mandatory2DGameFramework.Model.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.models.attack
{
    /*!
 * \interface IAttackItem
 * \brief Interface representing an attack item in the game world.
 * 
 */
    public interface IAttackItem : IWorldObject
    {
        /*!
         * \brief Gets or sets the type of damage this attack item deals.
         */
        DamageType DamageType { get; set; }
        /*!
        * \brief Gets or sets the attack type of the item (e.g., melee or ranged).
        */
        AttackType AttackType { get; set; }
        /*!
      * \brief Gets or sets the maximum damage this item can deal.
      */
        public int MaxDamage { get; set; }
    }
}
