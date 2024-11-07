
using Mandatory2DGameFramework.Model.Worlds;
using Mandatory2DGameFramework.Models;
using System.Diagnostics;

namespace Mandatory2DGameFramework.models.Creatures
{
    /*!
    * \brief Observes a Creature and handles notifications when certain events occur.
    * 
    * The CreatureObserver class allows external objects to subscribe to a creature's events. 
    * Currently, it handles the creature's death event, logging a message when the creature dies.
    */
    public class CreatureObserver
    {
        private World world;
        /*!
 * \brief Initializes a new instance of the CreatureObserver class.
 * 
 * \param world The game world that the observer can interact with.
 */
        public CreatureObserver(World world)
        {
            this.world = world;
        }
        /*!
     * \brief Subscribes the observer to a creature's death event.
     * 
     * This method attaches the observer to the specified creature's \c Died event, enabling 
     * the observer to receive notifications when the creature dies.
     * 
     * \param creature The Creature instance to subscribe to.
     */
        public void SubscribeToCreature(Creature creature)
        {
            MyLogger.TraceInformation($"Subscribing to {creature.Name}'s death event.");
            creature.Died += OnCreatureDied;
        }
        /*!
 * \brief Handles the creature's \c Died event.
 * 
 * This private method is triggered when a subscribed creature's \c Died event occurs.
 * It logs a message to the console indicating the creature has died.
 * 
 * \param sender The source of the event, expected to be of type Creature.
 * \param e Event arguments associated with the creature's death event.
 */
        private void OnCreatureDied(object? sender, EventArgs e)
        {
            if (sender is Creature creature)
            {
                MyLogger.TraceInformation($"{creature.Name} has died.");
                world._creatures.Remove(creature);
                MyLogger.TraceInformation($"{creature.Name} has been removed from the world.");
                if (creature.Usable != null)
                {
                    world.AddOnLocation(creature.PosX, creature.PosY, creature.Usable);
                    MyLogger.TraceInformation($"{creature.Name} has dropped {creature.Usable.Name}.");
                }

            }
        }
    }
}

