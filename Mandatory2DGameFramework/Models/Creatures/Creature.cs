using Mandatory2DGameFramework.models.attack;
using Mandatory2DGameFramework.models.Creatures.States;
using Mandatory2DGameFramework.models.defence;
using Mandatory2DGameFramework.models.usable;
using Mandatory2DGameFramework.Model.Worlds;
using System.Diagnostics;
using Mandatory2DGameFramework.Models;


namespace Mandatory2DGameFramework.models.Creatures
{
    /*!
    * \brief Represents a creature within the game, capable of moving, attacking, looting
    *
    * It can interact with other objects and other creatures in the world and transitions between states based on its behavior (e.g., moving or attacking).
    */
    public class Creature : ICreature
    {
        /*!
         * \brief Event triggered when the creature dies.
         */
        public event EventHandler<EventArgs>? Died;
        private HashSet<Tuple<int, int>> triedCoordinates = new HashSet<Tuple<int, int>>();
        private int turnCounter = 0;
        private int maxTurns = 10;

        private ICreatureState currentState;
        /*!
        * \brief Gets or sets the current state of the creature.
        */
        public ICreatureState CurrentState
        {
            get => currentState;
            set => currentState = value;
        }

        /*!
        * \brief The name of the creature.
        */
        public string Name { get; set; }

        /*!
         * \brief The X-coordinate position of the creature.
         */
        public int PosX { get; set; }
        /*!
         * \brief The Y-coordinate position of the creature.
         */
        public int PosY { get; set; }

        /*!
       * \brief The creature's attack item, if any.
       */
        public IAttackItem? Attack { get; set; }
        /*!
         * \brief The creature's defence item, if any.
         */
        public IDefenceItem? Defence { get; set; }
        /*!
        * \brief The creature's usable item, if any.
        */
        public IUsableItem? Usable { get; set; }

        /*!
         * \brief The creature's current hit points.
         */
        public int HitPoint { get; set; }
        /*!
         * \brief The creature's maximum hit points.
         */
        public int MaxHitPoints { get; set; }
        /*!
         * \brief The creature's armor class, used in determining damage resistance.
         */
        public int ArmorClass { get; set; }
        /*!
         * \brief The creature's strength attribute.
         */
        public int Strength { get; set; }
        /*!
         * \brief The creature's dexterity attribute.
         */
        public int Dexterity { get; set; }

        private bool alive;
        /*!
         * \brief Gets or sets the alive status of the creature.
         * 
         * Setting this to false triggers the Died event.
         */
        public bool Alive
        {
            get => alive;
            set
            {
                if (alive != value)
                {
                    alive = value;
                    if (!alive)
                    {
                        OnDied(); // Notify observers if the creature dies
                    }
                }
            }
        }

        /*!
         * \brief Indicates if the creature can be looted. - Will be false, as creatures can't be looted.
         */
        public bool Lootable { get; set; }
        /*!
         * \brief Indicates if the creature can be removed from the world. - Will be false, as creatures can't be removed.
         */
        public bool Removeable { get; set; }


        /*!
     * \brief Initializes a new instance of the Creature class.
 * \param name The name of the creature.
 * \param hitPoint The starting and maximum hit points of the creature.
 * \param armorClass The armor class of the creature.
 * \param strength The strength attribute of the creature.
 * \param dexterity The dexterity attribute of the creature.
 * 
 */
        public Creature(string name, int hitPoint, int armorClass, int strength, int dexterity)
        {
            Name = name;
            HitPoint = hitPoint;
            MaxHitPoints = hitPoint;
            ArmorClass = armorClass;
            Strength = strength;
            Dexterity = dexterity;
            Alive = true;
            Lootable = false;
            Removeable = false;

            currentState = new MoveState();
        }


        /*!
          * \brief Handles the creature's turn, managing its interactions with the world.
          * 
          * The creature will continue interacting with objects in the world until it has completed an action.
          * \param world The world containing all objects the creature may interact with.
          */
        public void TakeTurn(World world)
        {
            MyLogger.TraceInformation($"Creature {Name} is taking a turn.");
            TransitionTo(new MoveState());
            /*\brief List of objects that the creature has already interacted with in the current turn. Helps to avoid an infinite loop, if the item is not preferred. 
             */
            List<IWorldObject> interactdObjects = new List<IWorldObject>();

            // Continue the turn until the creature has done something
            bool turnDone = false;

            while (!turnDone)
            {
                var closestObject = world.GetClosestInteractableObject(this, interactdObjects);

                if (closestObject == null)
                {
                    // No objects to interact with - end turn
                    turnDone = true;
                    break;
                }

                MyLogger.TraceInformation($"Handling turn");
                turnDone = currentState.Handle(this, world, closestObject);

                // Ensure the object is not interacted with again in the same turn if it was not preferred or could not be interacted with
                interactdObjects.Add(closestObject);

            }
        }
        /*!
       * \brief Checks if an item is preferred based on the creature's current equipment.
       * 
       * \param item The item to check.
       * \return True if the item is preferred; otherwise, false.
       */
        public bool IsItemPreferred(IWorldObject item)
        {
            if (item is MeleeAttackItem)
            {
                if (Attack == null)
                {
                    return true;
                }
                else
                {
                    var attackitem = item as MeleeAttackItem;
                    if (attackitem != null && Attack.MaxDamage + Strength >= attackitem.MaxDamage + Strength)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else if (item is RangedAttackItem)
            {
                if (Attack == null)
                {
                    return true;
                }
                else
                {
                    var attackitem = item as RangedAttackItem;
                    if (attackitem != null && Attack.MaxDamage + Dexterity >= attackitem.MaxDamage + Dexterity)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else if (item is DefenceItem)
            {
                if (Defence == null)
                {
                    return true;
                }
                else
                {
                    var defenceItem = item as DefenceItem;
                    if (defenceItem != null && Defence.ArmorClass >= defenceItem.ArmorClass)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else if (item is IUsableItem)
            {
                if (Usable == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        /*!
        * \brief Checks if the creature can loot the specified object.
        * 
        * \param obj The object to check.
        * \param world The world context for checking distance.
        * \return True if the creature can loot the object; otherwise, false.
        */
        public bool CanLoot(IWorldObject obj, World world)
        {
            if (world.GetDistance(PosX, PosY, obj.PosX, obj.PosY) <= 1)
            {
                return true;
            }
            else return false;
        }
        /*!
        * \brief Checks if the creature can attack the specified enemy.
        * 
        * \param enemy The enemy creature to check.
        * \param world The world context for checking distance.
        * \return True if the creature can attack; otherwise, false.
        */

        public bool CanAttack(ICreature enemy, World world)
        {
            if (!enemy.Alive)
            {
                // Can't attack dead creatures
                return false;
            }

            int attackRange = GetAttackRange();
            int distanceToEnemy = world.GetDistance(PosX, PosY, enemy.PosX, enemy.PosY);

            if (distanceToEnemy > attackRange)
            {
                MyLogger.TraceInformation($"Creature {Name} can't attack {enemy.Name} because it is out of range.");
                return false;
            }

            return true;
        }
        /*!
        * \brief Moves the creature towards a specified object.
        * 
        * If the path is blocked, it finds an alternative route within the world boundaries.
        * \param obj The target object to move towards.
        * \param world The world context for checking positions.
        */

        public void MoveTowardsObj(IWorldObject obj, World world)
        {
            MyLogger.TraceInformation($"Creature {Name} is moving towards {obj.Name}.");
            // direction to the move
            int deltaX = obj.PosX - PosX;
            int deltaY = obj.PosY - PosY;
            // Speed / turn
            // todo: implement in class instead
            int MovementSpeed = 1;
            int stepX = 0;
            int stepY = 0;
            if (Math.Abs(deltaX) > Math.Abs(deltaY))
            {
                stepX = Math.Sign(deltaX) * Math.Min(Math.Abs(deltaX), MovementSpeed);
            }
            else
            {
                stepY = Math.Sign(deltaY) * Math.Min(Math.Abs(deltaY), MovementSpeed);
            }
            int newX = PosX + stepX;
            int newY = PosY + stepY;
            if (!world.LocationFree(newX, newY))
            {
                // If the location is occupied, we need to find another free location.
                List<Tuple<int, int>> potentialMoves = new List<Tuple<int, int>>();
                // Try moving in different directions to find an empty spot
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        if (dx == 0 && dy == 0) continue;
                        int checkX = PosX + dx;
                        int checkY = PosY + dy;
                        // Check if the position is within bounds and is free
                        if (world.LocationFree(checkX, checkY) && checkX > 0 && checkX <= world.MaxX && checkY > 0 && checkY <= world.MaxY && !triedCoordinates.Contains(Tuple.Create(checkX, checkY)))
                        {
                            potentialMoves.Add(Tuple.Create(checkX, checkY));
                        }
                    }
                }
                // If we have valid moves, choose the closest one to the enemy
                if (potentialMoves.Count > 0)
                {
                    var closestMove = potentialMoves
                        .OrderBy(m => world.GetDistance(m.Item1, m.Item2, obj.PosX, obj.PosY)) // Sort by distance to enemy
                        .First();
                    newX = closestMove.Item1;
                    newY = closestMove.Item2;
                    triedCoordinates.Add(Tuple.Create(newX, newY));
                }
                else
                {
                    MyLogger.TraceError($"Creature {Name} can't move towards {obj.Name} because all paths are blocked.");
                    // If no valid moves, the creature can't move (this could be a fallback, or we could randomize)
                    return;
                }
            }

            // Update the position of the creature after moving
            PosX = newX;
            PosY = newY;
            turnCounter++;
            if (turnCounter >= maxTurns)
            {
                turnCounter = 0;
                triedCoordinates.Clear();
            }

        }

        /*!
         * \brief Gets the attack range of the creature based on its equipped attack item.
         * 
         * \return The range at which the creature can attack.
         */

        private int GetAttackRange()
        {
            if (Attack==null)
            {
                return 1;
            }
            else if (Attack.AttackType == AttackType.melee)
            {
                return 1;
            }
            else if (Attack.AttackType == AttackType.ranged)
            {
                return 3;
            }
            return 1;
        }
        /*!
       * \brief Performs an attack on the specified enemy creature.
       * 
       * \param enemy The creature to attack.
       */

        public void Hit(ICreature enemy)
        {
            
            MyLogger.TraceInformation($"Creature {Name} is attacking {enemy.Name}.");
            Random random = new Random();
            if (Attack != null)
            {
                MyLogger.TraceInformation($"Creature {Name} is attacking {enemy.Name} with {Attack}.");
                if (Attack.DamageType == DamageType.strength)
                {
                    int toHit = random.Next(1, 20) + Strength;
                    int attack = random.Next(1, Attack.MaxDamage) + Strength;
                    MyLogger.TraceInformation($"Creature {Name} hit {enemy.Name} with {toHit} to hit and {Attack} to attack");
                    enemy.ReceiveHit(toHit, attack);

                }
                else if (Attack.DamageType == DamageType.dexterity)
                {

                    int toHit = random.Next(1, 20) + Dexterity;
                    int attack = random.Next(1, Attack.MaxDamage) + Dexterity;
                    MyLogger.TraceInformation($"Creature {Name} hit {enemy.Name} with {toHit} to hit and {Attack} to attack");
                    enemy.ReceiveHit(toHit, attack);
                }
            }
            else
            {
                int toHit = random.Next(1, 20);
                int attack = Strength;
                MyLogger.TraceInformation($"Creature {Name} hit {enemy.Name} with {toHit} to hit and {Attack} to attack");
                enemy.ReceiveHit(toHit, attack);
            }
        }
        /*!
      * \brief Handles receiving damage from an attack.
      * 
      * Reduces hit points if the attack succeeds against the creature's armor class.
      * \param toHit The attack roll to determine if the attack hits.
      * \param damage The amount of damage if the attack hits.
      */

        public void ReceiveHit(int toHit, int damage)
        {
            int totalArmorClass = ArmorClass;
            if (Defence != null)
            {
                totalArmorClass += Defence.ArmorClass;
            }
            if (toHit >= totalArmorClass)
            {
                HitPoint -= damage;
                MyLogger.TraceInformation($"Creature {Name} was hit for {damage} damage. Current hp: {HitPoint}");
                if (HitPoint <= 0)
                {
                    Alive = false;
                }
            }
            else
            {
                MyLogger.TraceInformation($"Creature {Name} was missed.");
            }
        }
        /*!
         * \brief Loots an item from the world and equips it if applicable.
         * 
         * \param obj The item to loot.
         * \param world The world context for removing looted items.
         */

        public void Loot(IWorldObject obj, World world)
        {
            if (obj is IAttackItem attackItem)
            {
                Attack = attackItem;
                MyLogger.TraceInformation($"Creature {Name} looted {obj.ToString()}");
                world._AttackItems.Remove(attackItem);
            }
            else if (obj is IDefenceItem defenceItem)
            {
                Defence = defenceItem;
                MyLogger.TraceInformation($"Creature {Name} looted {obj.ToString()}");
                world._defenceItems.Remove(defenceItem);
            }
        }
        /*!
        * \brief Triggers the Died event when the creature dies.
        */
        protected virtual void OnDied()
        {
            Died?.Invoke(this, EventArgs.Empty);
        }

        /*!
         * \brief Drops an item the creature is carrying.
         */
        // todo: Implement
        public void DropItem()
        {
            throw new NotImplementedException();
        }

        /*!
         * \brief Transitions the creature to a new state.
         * 
         * \param state The new state to transition to.
         */

        public void TransitionTo(ICreatureState state)
        {
            MyLogger.TraceInformation($"Creature {Name} is transitioning to {state.GetType().Name} state.");
            currentState = state;
        }

        public void ExecuteState(ICreature creature, World world, IWorldObject obj)
        {
            currentState.Handle(creature, world, obj);
        }
        /*!
        * \brief Returns a string representation of the creature's status.
        * 
        * \return A string summarizing key attributes of the creature.
        */


        public override string ToString()
        {
            return $"{{{nameof(Name)}={Name}, {nameof(HitPoint)}={HitPoint.ToString()}, {nameof(Attack)}={Attack}, {nameof(Defence)}={Defence}, {nameof(ArmorClass)}={ArmorClass}}}";
        }
    }
}
