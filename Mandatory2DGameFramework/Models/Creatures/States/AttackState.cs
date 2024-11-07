using Mandatory2DGameFramework.Model.Worlds;
using Mandatory2DGameFramework.Models;
namespace Mandatory2DGameFramework.models.Creatures.States
{
    /*! 
     * \brief Represents the "Attack" state of a creature.
     * The state will be activated when the creature is in range on a opponent and has performed an attack check. 
     * The attack is executed when the 'Handle' methods is called.
     */
    public class AttackState : ICreatureState
    {
        public bool Handle(ICreature creature, World world, IWorldObject obj)
        {
            /*!
             * \brief is called when the creature enters attack state.
             * Will call the 'Hit' method on the creature object.
             * Returns that the Creature has performed an action on its turn, so that the turn can end. 
             */
            MyLogger.TraceInformation($"{creature.Name} attacks {obj.Name}.");  
            
                creature.Hit(obj as Creature);
                return true;

        }
    }
}
