using Mandatory2DGameFramework.models.attack;
using Mandatory2DGameFramework.models.Creatures.States;
using Mandatory2DGameFramework.models.defence;
using Mandatory2DGameFramework.models.usable;
using Mandatory2DGameFramework.Model.Worlds;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.models.Creatures
{
    public class Creature : ICreature
    {
        public event EventHandler<EventArgs>? Died;
        private HashSet<Tuple<int, int>> triedCoordinates = new HashSet<Tuple<int, int>>();
        private int turnCounter = 0;
        private int maxTurns = 10;

        private ICreatureState currentState;
        public ICreatureState CurrentState
        {
            get => currentState;
            set => currentState = value;
        }

        public string Name { get; set; }

        // Position of the creature
        public int PosX { get; set; }
        public int PosY { get; set; }

        // Attack and defence item of the creature if any
        public IAttackItem? Attack { get; set; }
        public IDefenceItem? Defence { get; set; }
        public IUsableItem? Usable { get; set; }


        public int HitPoint { get; set; }
        public int MaxHitPoints { get; set; }
        public int ArmorClass { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }

        private bool alive;
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


        public bool Lootable { get; set; }
        public bool Removeable { get; set; }



        public Creature(string name, int hitPoint)
        {
            Name = name;
            HitPoint = hitPoint;
            MaxHitPoints = hitPoint;
            Alive = true;
            Lootable = false;
            Removeable = false;
        }


        // Method for handling the creatures turn
        public void TakeTurn(World world)
        {
            currentState = new MoveState();
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

                turnDone = currentState.Handle(this, world, closestObject);
                interactdObjects.Add(closestObject);

            }
        }

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
                    if (Attack.MaxDamage + Strength >= attackitem.MaxDamage + Strength)
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
                    if (Attack.MaxDamage + Dexterity >= attackitem.MaxDamage + Dexterity)
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
                    if (Defence.ArmorClass >= defenceItem.ArmorClass)
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
        public bool CanLoot(IWorldObject obj, World world)
        {
            if(world.GetDistance(PosX,PosY,obj.PosX,obj.PosY) <= 1)
            {
                return true;
            }
            else return false;
        }

        public bool CanAttack(ICreature enemy, World world)
        {
            if (!enemy.Alive)
            {
                // Can't attack dead creatures
                return false;
            }

            int attackRange = GetAttackRange();
            int distanceToEnemy = world.GetDistance(PosX, PosY, enemy.PosX, enemy.PosY);
            Trace.TraceInformation($"Creature {Name} is {distanceToEnemy} tiles away from {enemy.Name}.");

            if (distanceToEnemy > attackRange)
            {
                Trace.TraceInformation($"Creature {Name} can't attack {enemy.Name} because it is out of range.");
                return false;
            }

            return true;
        }

        public void MoveTowardsObj(IWorldObject obj, World world)
        {
            bool haveMoved = false;
            // direction to the move
            int deltaX = obj.PosX - PosX;
            int deltaY = obj.PosY - PosY;

            // Speed / turn
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
                        if (dx == 0 && dy == 0) continue; // Skip the original position

                        int checkX = PosX + dx;
                        int checkY = PosY + dy;

                        // Check if the position is within bounds and is free (not occupied)
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

        public void Hit(ICreature enemy)
        {
            Random random = new Random();
            if (Attack != null)
            {
                if (Attack.DamageType == DamageType.strength)
                {
                    int toHit = random.Next(1, 20) + Strength;
                    int attack = random.Next(1, Attack.MaxDamage) + Strength;
                    Trace.TraceInformation($"Creature {Name} hit {enemy.Name} with {toHit} to hit and {Attack} to attack");
                    Trace.Flush();
                    enemy.ReceiveHit(toHit, attack);

                }
                else if (Attack.DamageType == DamageType.dexterity)
                {

                    int toHit = random.Next(1, 20) + Dexterity;
                    int attack = random.Next(1, Attack.MaxDamage) + Dexterity;
                    Trace.TraceInformation($"Creature {Name} hit {enemy.Name} with {toHit} to hit and {Attack} to attack");
                    Trace.Flush();

                    enemy.ReceiveHit(toHit, attack);
                }
            }
            else
            {
                int toHit = random.Next(1, 20);
                int attack = Strength;
                Trace.TraceInformation($"Creature {Name} hit {enemy.Name} with {toHit} to hit and {Attack} to attack");
                Trace.Flush();
                enemy.ReceiveHit(toHit, attack);
            }
        }

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
                if (HitPoint <= 0)
                {
                    Alive = false;
                }
            }
        }

        public void Loot(IWorldObject obj, World world)
        {
            if (obj.GetType() == typeof(MeleeAttackItem) || obj.GetType() == typeof(RangedAttackItem))
            {
                if (Attack != null)
                {

                }
                Attack = obj as IAttackItem;
                Trace.TraceInformation($"Creature {Name} looted {obj.ToString()}");
                world._AttackItems.Remove(obj as IAttackItem);
            }
            else if (obj.GetType() == typeof(IDefenceItem))
            {
                Defence = obj as IDefenceItem;
            }
        }
        protected virtual void OnDied()
        {
            Died?.Invoke(this, EventArgs.Empty);
        }

        public void DropItem()
        {

        }

        public void TransitionTo(ICreatureState state)
        {
            currentState = state;
        }

        public override string ToString()
        {
            return $"{{{nameof(Name)}={Name}, {nameof(HitPoint)}={HitPoint.ToString()}, {nameof(Attack)}={Attack}, {nameof(Defence)}={Defence}}}";
        }
    }
}
