using Mandatory2DGameFramework.models.attack;
using Mandatory2DGameFramework.models.Creatures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Model.Worlds
{
    public class World
    {
        public int MaxX { get; set; }
        public int MaxY { get; set; }


        public List<IWorldObject> _worldObjects;
        public List<ICreature> _creatures;
        public List<IAttackItem> _AttackItems;

        public World(int maxX, int maxY)
        {
            MaxX = maxX;
            MaxY = maxY;
            _worldObjects = new List<IWorldObject>();
            _AttackItems = new List<IAttackItem>();
            _creatures = new List<ICreature>();
        }

        public bool LocationFree(int x, int y)
        {
            var allObjects = new List<IWorldObject>();
            allObjects.AddRange(_worldObjects);
            allObjects.AddRange(_AttackItems);
            allObjects.AddRange(_creatures);
            foreach (var worldObject in allObjects)
            {
                if (worldObject.PosX == x && worldObject.PosY == y)
                {
                    Trace.TraceInformation($"Location {x},{y} is not free.");
                    Trace.Flush();
                    return false;
                }
            }
            return true;
        }

        public void AddOnLocation(int x, int y, object toAdd)
        {
            if (LocationFree(x, y))
            {
                if (toAdd.GetType().Equals(typeof(WorldObject)))
                {
                    WorldObject obj = toAdd as WorldObject;
                    _worldObjects.Add(obj);
                    obj.PosX = x;
                    obj.PosY = y;
                    Trace.TraceInformation($"Object {toAdd.ToString()} added on {x},{y}.");
                    Trace.Flush();
                }
                else if (toAdd.GetType().Equals(typeof(Creature)))
                {
                    Creature creature = toAdd as Creature;
                    _creatures.Add(toAdd as Creature);
                    creature.PosX = x;
                    creature.PosY = y;
                    Trace.TraceInformation($"Creature {toAdd.ToString()} added on {x},{y}.");
                    Trace.Flush();
                }
                else if (toAdd.GetType().Equals(typeof(MeleeAttackItem)) || toAdd.GetType().Equals(typeof(RangedAttackItem)))
                {
                    IAttackItem attackItem = toAdd as IAttackItem;
                    _AttackItems.Add(attackItem);
                    attackItem.PosX = x;
                    attackItem.PosY = y;
                    Trace.TraceInformation($"AttackItem {toAdd.ToString()} added on {x},{y}.");
                    Trace.Flush();
                }
                else
                {
                    Trace.TraceError("Item could not be added due to incompatible type");
                    Trace.Flush();
                }
            }

        }

        public object GetObjectOnLocation(int x, int y)
        {
            var allObjects = new List<IWorldObject>();

            // Get obejct from _worldObjects filtering only lootables using LINQ
            allObjects.AddRange(_worldObjects);
            allObjects.AddRange(_AttackItems);
            allObjects.AddRange(_creatures);
            foreach (var worldObject in allObjects)
            {
                if (worldObject.PosX == x && worldObject.PosY == y)
                {
                    return worldObject;
                }
            }
            return null;
        }

        public ICreature GetClosestEnemy(ICreature creature)
        {
            // Logic to find closest enemy
            ICreature closestEnemy = null;
            double closestDistance = double.MaxValue;

            foreach (var enemy in _creatures)
            {
                if (enemy != creature && enemy.Alive)
                {
                    double distance = GetDistance(creature.PosX, creature.PosY, enemy.PosX, enemy.PosY);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestEnemy = enemy;
                    }
                }
            }
            return closestEnemy;
        }

        public IWorldObject GetClosestInteractableObject(ICreature creature, List<IWorldObject> interactedObjects)
        {
            // Logic to find closest world object
            IWorldObject closestObj = null;
            double closestDistance = double.MaxValue;

            var allObjects = new List<IWorldObject>();

            // Get obejct from _worldObjects filtering only lootables using LINQ
            allObjects.AddRange(_worldObjects.Where(obj => obj.Lootable));
            allObjects.AddRange(_AttackItems);
            allObjects.AddRange(_creatures.Where(obj => obj != creature));

            // Filters out objects that have already been interacted with, to avoid infinite loop
            var validObjects = allObjects.Except(interactedObjects);

            foreach (var obj in allObjects)
            {
                // Calculate distance between creature and object
                double distance = GetDistance(creature.PosX, creature.PosY, obj.PosX, obj.PosY);

                // If distance is less than the closest distance, set the object as the closest object
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObj = obj;
                }
            }
            // Return the closest object
            return closestObj;
        }

        public int GetDistance(int x1, int y1, int x2, int y2)
        {
            // Using Chebyshev distance for 8 directional movement
            // Decision made to account for diagonal movement in grid
            Trace.TraceInformation($"Calculating distance between {x1},{y1} and {x2},{y2}.");
            Trace.TraceInformation($"Distance is {Math.Max(Math.Abs(x1 - x2), Math.Abs(y1 - y2))}.");
            return Math.Max(Math.Abs(x1 - x2), Math.Abs(y1 - y2));
        }

        public override string ToString()
        {
            return $"{{{nameof(MaxX)}={MaxX.ToString()}, {nameof(MaxY)}={MaxY.ToString()}, {nameof(_worldObjects)}={string.Join("," + _worldObjects)} }}";
        }


    }
}
