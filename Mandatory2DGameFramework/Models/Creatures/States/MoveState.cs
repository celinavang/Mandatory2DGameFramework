using Mandatory2DGameFramework.models.attack;
using Mandatory2DGameFramework.models.defence;
using Mandatory2DGameFramework.models.usable;
using Mandatory2DGameFramework.Model.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.models.Creatures.States
{
    public class MoveState : ICreatureState
    {
        public bool Handle(ICreature creature, World world, IWorldObject obj)
        {
            if (obj is Creature)
            {
                if (creature.CanAttack(obj as Creature, world))
                {
                    creature.TransitionTo(new AttackState());
                }
                else
                {
                    creature.MoveTowardsObj(obj, world);
                    return true;
                }
            }
            else if (obj is MeleeAttackItem || obj is RangedAttackItem || obj is DefenceItem || obj is IUsableItem)
            {
                if (creature.IsItemPreferred(obj))
                {
                    if (creature.CanLoot(obj, world))
                    {
                        creature.Loot(obj, world);
                        return true;
                    }
                    else
                    {
                        creature.MoveTowardsObj(obj, world);
                        return true;
                    }

                }

            }
            return false;

        }
    }


}
