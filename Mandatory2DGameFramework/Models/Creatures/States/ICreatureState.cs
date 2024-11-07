using Mandatory2DGameFramework.Model.Worlds;

namespace Mandatory2DGameFramework.models.Creatures.States
{
    /*! 
     * \brief Interface for the different states a creature can be in.
     * The states are used to determine what action the creature should perform on its turn. 
     */
    public interface ICreatureState
    {
        /*!
         * \brief Handles the state of the creature.
         * The method will be called when the creature is in the state and it is the creatures turn.
         * The method will return a boolean value, which will determine if the creature has performed an action on its turn. 
         * \param creature A reference to the creature that is in the state. This is the creature whose behavior is being handled.
         * \param world The world in which the creature and the object exist. This allows checking for collisions and interactions with the environment.
         * \param obj The object that the creature is interacting with. This can be another creature or an item.
         * 
         */
        bool Handle(ICreature creature, World world, IWorldObject obj);
    }
}
