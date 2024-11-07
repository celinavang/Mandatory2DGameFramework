using Mandatory2DGameFramework.models.attack;
using Mandatory2DGameFramework.models.Creatures.States;
using Mandatory2DGameFramework.models.defence;
using Mandatory2DGameFramework.Model.Worlds;


namespace Mandatory2DGameFramework.models.Creatures
{
    /*!
     * \interface ICreature
     * \brief Defines the interface for creatures in the game world, including their attributes, actions, and interactions with other objects.
     * 
     * This interface provides methods for attacking, defending, moving, looting, and managing the creature's state.
     */
    public interface ICreature : IWorldObject
    {
        // The creature's equipped attack item
        IAttackItem? Attack { get; set; }
        // The creature's equipped defense item
        IDefenceItem? Defence { get; set; }
        // The creature's current hit points
        int HitPoint { get; set; }
        // The creature's maximum hit points
        int MaxHitPoints { get; set; }
        // The creature's armor class (defense ability)
        int ArmorClass { get; set; }
        // The creature's strength attribute
        int Strength { get; set; }
        // The creature's dexterity attribute
        int Dexterity { get; set; }
        // Indicates whether the creature is alive
        bool Alive { get; set; }
        /*!
         * \method Hit
         * \brief Inflicts damage on the target creature.
         * \param enemy The creature being attacked.
         */
        void Hit(ICreature enemy);
        /*!
         * \method ReceiveHit
         * \brief Processes the damage received from an attack.
         * \param toHit The attack roll result.
         * \param damage The amount of damage dealt.
         */
        void ReceiveHit(int toHit, int damage);
        /*!
         * \method Loot
         * \brief Allows the creature to loot an item from the world.
         * \param obj The object to loot.
         * \param world The world context.
         */
        void Loot(IWorldObject obj, World world);

        void DropItem();
        /*!
                 * \method CanAttack
                 * \brief Determines if the creature can attack another creature.
                 * \param enemy The enemy creature to check.
                 * \param world The world context.
                 * \return True if the creature can attack, false otherwise.
                 */
        bool CanAttack(ICreature enemy, World world);
        /*!
         * \method MoveTowardsObj
         * \brief Moves the creature towards a specified object in the world.
         * \param obj The object to move towards.
         * \param world The world context.
         */
        void MoveTowardsObj(IWorldObject obj, World world);
        /*!
         * \method IsItemPreferred
         * \brief Determines if the creature prefers a certain item over the currently equipped item.
         * \param obj The object to check.
         * \return True if the creature prefers the item, false otherwise.
         */
        bool IsItemPreferred(IWorldObject obj);
        /*!
       * \method CanLoot
       * \brief Checks if the creature can loot a given object.
       * \param obj The object to loot.
       * \param world The world context.
       * \return True if the creature can loot the object, false otherwise.
       */
        bool CanLoot(IWorldObject obj, World world);
        /*!
         * \method TransitionTo
         * \brief Changes the creature's state (e.g., idle, attacking, fleeing).
         * \param state The new state to transition to.
         */
        void TransitionTo(ICreatureState state);
        void ExecuteState(ICreature creature, World world, IWorldObject obj);
        /*!
         * \method TakeTurn
         * \brief Executes the creature's turn in the game, performing actions like attacking, moving, or interacting.
         * \param world The world context.
         */

        void TakeTurn(World world);
    }
}
