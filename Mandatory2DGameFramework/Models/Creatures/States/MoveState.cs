using Mandatory2DGameFramework.models.attack;
using Mandatory2DGameFramework.models.defence;
using Mandatory2DGameFramework.models.usable;
using Mandatory2DGameFramework.Model.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mandatory2DGameFramework.Models;

namespace Mandatory2DGameFramework.models.Creatures.States
{
    /*! 
     * \brief The MoveState class handles the behavior of a creature when it is in a state of moving towards an object.
     * 
     * The state is activated when the creature begins its turn. 
     * The movement is calculated based on the relative position of the object, and the creature will either move towards the object or 
     * transition to another state (e.g., attacking) if it can interact with the object.
     */
    public class MoveState : ICreatureState
    {
        /*!
         * \brief Handles the state behavior when the creature is moving towards an object.
         * 
         * This method is called during the creature's turn when it is in the MoveState. The creature will either move towards the object, 
         * attempt to attack it, or interact with it if it is lootable.
         * 
         * \return Returns a boolean value:
         *         - \c true if the creature performed an action (either moved, attacked, or looted).
         *         - \c false if no action was performed (the creature could not move or interact with the object).
         */
        public bool Handle(ICreature creature, World world, IWorldObject obj)
        {
            
            // Check if the object is a creature
            if (obj is Creature)
            {
                if(creature.Attack == null && world._AttackItems.Any())
                {
                    return false;
                }
                else if(creature.Defence == null && world._defenceItems.Any())
                {
                    return false;
                }
                // If the creature can attack the target - meaning it is in range of the target - transition to AttackState
                else if (creature.CanAttack(obj as Creature, world))
                {
                    MyLogger.TraceInformation($"{creature.Name} attacks {obj.Name}.");
                    creature.TransitionTo(new AttackState());
                    creature.ExecuteState(creature, world, obj);
                    return true;
                }
                else
                {
                    // Otherwise, move the creature towards the object (creature)
                    creature.MoveTowardsObj(obj, world);
                    return true;
                }
            }
            // Check if the object is an item (Melee/Ranged Attack, Defence, or Usable)
            else if (obj is MeleeAttackItem || obj is RangedAttackItem || obj is DefenceItem || obj is IUsableItem)
            {
                // If the item is preferred by the creature - meaning it is better than the one it already owns, if it owns any - attempt to loot it
                if (creature.IsItemPreferred(obj))
                {
                    // Check if the creature can loot the item - it has to be in range of the item
                    if (creature.CanLoot(obj, world))
                    {
                        MyLogger.TraceInformation($"{creature.Name} loots {obj.Name}.");
                        creature.Loot(obj, world);
                        return true;
                    }
                    else
                    {
                        // If the creature cannot loot, move towards the item
                        creature.MoveTowardsObj(obj, world);
                        return true;
                    }

                }

            }
            // Return false if no action was performed - the Turn will loop
            return false;

        }
    }


}
