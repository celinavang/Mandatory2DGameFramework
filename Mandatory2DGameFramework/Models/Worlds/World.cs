using Mandatory2DGameFramework.models.attack;
using Mandatory2DGameFramework.models.Creatures;
using Mandatory2DGameFramework.models.defence;
using Mandatory2DGameFramework.Models;
using System.Diagnostics;

namespace Mandatory2DGameFramework.Model.Worlds
{
    /*!
* \class World
* \brief Represents the game world, holding and managing all world objects, creatures, and attack items.
* It handles object placement, interaction, and distance calculations within the world grid.
*/
    public class World
    {

        // Maximum coordinates for the world grid
        public int MaxX { get; set; }
        public int MaxY { get; set; }

        // Collections of world objects, creatures, and attack items
        public List<IWorldObject> _worldObjects;
        public List<ICreature> _creatures;
        public List<IAttackItem> _AttackItems;
        public List<IDefenceItem> _defenceItems;

        /*!
         * \constructor World
         * \brief Initializes a new world with specified dimensions.
         * \param maxX The maximum X-coordinate of the world.
         * \param maxY The maximum Y-coordinate of the world.
         */
        public World(int maxX, int maxY)
        {
            MaxX = maxX;
            MaxY = maxY;
            _worldObjects = new List<IWorldObject>();
            _AttackItems = new List<IAttackItem>();
            _creatures = new List<ICreature>();
            _defenceItems = new List<IDefenceItem>();
        }

        /*!
         * \method LocationFree
         * \brief Checks if a location on the world grid is free (unoccupied).
         * \param x The X-coordinate of the location.
         * \param y The Y-coordinate of the location.
         * \return True if the location is free, false otherwise.
         */
        public bool LocationFree(int x, int y)
        {
            var allObjects = new List<IWorldObject>();
            allObjects.AddRange(_worldObjects);
            allObjects.AddRange(_AttackItems);
            allObjects.AddRange(_creatures);
            allObjects.AddRange(_defenceItems);
            foreach (var worldObject in allObjects)
            {
                if (worldObject.PosX == x && worldObject.PosY == y)
                {
                    return false;
                }
            }
            return true;
        }
        /*!
         * \method AddOnLocation
         * \brief Adds an object (world object, creature, or attack item) to a specific location on the world grid.
         * \param x The X-coordinate of the location.
         * \param y The Y-coordinate of the location.
         * \param toAdd The object to be added (must be a valid type).
         */
        public void AddOnLocation(int x, int y, object toAdd)
        {
            if (LocationFree(x, y))
            {
                if (toAdd is WorldObject obj)
                {
                    _worldObjects.Add(obj);
                    obj.PosX = x;
                    obj.PosY = y;
                    MyLogger.TraceInformation($"Object {toAdd.ToString()} added on {x},{y}.");
                }
                else if (toAdd is Creature creature)
                {
                    _creatures.Add(creature);
                    creature.PosX = x;
                    creature.PosY = y;
                    MyLogger.TraceInformation($"Creature {toAdd.ToString()} added on {x},{y}.");
                }
                else if (toAdd is IAttackItem attackItem)
                {
                    _AttackItems.Add(attackItem);
                    attackItem.PosX = x;
                    attackItem.PosY = y;
                    MyLogger.TraceInformation($"AttackItem {toAdd.ToString()} added on {x},{y}.");
                } else if (toAdd is IDefenceItem defenceItem)
                {
                    _defenceItems.Add(defenceItem);
                    defenceItem.PosX = x;
                    defenceItem.PosY = y;
                    MyLogger.TraceInformation($"DefenceItem {toAdd.ToString()} added on {x},{y}.");
                }
                else
                {
                    MyLogger.TraceError("Item could not be added due to incompatible type");
                }
            }
        }

        /*!
         * \method GetObjectOnLocation
         * \brief Retrieves the object located at the specified coordinates.
         * \param x The X-coordinate of the location.
         * \param y The Y-coordinate of the location.
         * \return The object located at the specified position, or null if no object is found.
         */
        public object? GetObjectOnLocation(int x, int y)
        {
            var allObjects = new List<IWorldObject>();

            // Find and return the object at the specified coordinates
            allObjects.AddRange(_worldObjects);
            allObjects.AddRange(_AttackItems);
            allObjects.AddRange(_creatures);
            allObjects.AddRange(_defenceItems);
            foreach (var worldObject in allObjects)
            {
                if (worldObject.PosX == x && worldObject.PosY == y)
                {
                    return worldObject;
                }
            }
            return null;
        }
        /*!
         * \method GetClosestEnemy
         * \brief Finds the closest enemy creature to the given creature.
         * \param creature The creature to find the closest enemy for.
         * \return The closest enemy creature or null if no enemies exist.
         */
        public ICreature? GetClosestEnemy(ICreature creature)
        {
            ICreature? closestEnemy = null;
            double closestDistance = double.MaxValue;
            // Calculate distance to all other creatures and return the closest enemy
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
        /*!
         * \method GetClosestInteractableObject
         * \brief Finds the closest interactable object (lootable or attackable) near the specified creature.
         * \param creature The creature for which to find the closest interactable object.
         * \param interactedObjects A list of objects that the creature has already interacted with.
         * \return The closest interactable object or null if none exist.
         */
        public IWorldObject? GetClosestInteractableObject(ICreature creature, List<IWorldObject> interactedObjects)
        {
            IWorldObject? closestObj = null;
            double closestDistance = double.MaxValue;

            var allObjects = new List<IWorldObject>();

            allObjects.AddRange(_worldObjects.Where(obj => obj.Lootable));
            allObjects.AddRange(_AttackItems);
            allObjects.AddRange(_defenceItems);
            allObjects.AddRange(_creatures.Where(obj => obj != creature));

            // Filters out objects that have already been interacted with, to avoid infinite loop
            var validObjects = allObjects.Except(interactedObjects);

            foreach (var obj in validObjects)
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
            MyLogger.TraceInformation($"Closest object to {creature.Name} is {closestObj?.Name} at {closestObj?.PosX},{closestObj?.PosY}.");
            // Return the closest object
            return closestObj;
        }
        /*!
 * \method GetDistance
 * \brief Calculates the Chebyshev distance between two points (x1, y1) and (x2, y2).
 * \param x1 The X-coordinate of the first point.
 * \param y1 The Y-coordinate of the first point.
 * \param x2 The X-coordinate of the second point.
 * \param y2 The Y-coordinate of the second point.
 * \return The calculated distance between the two points.
 */
        public int GetDistance(int x1, int y1, int x2, int y2)
        {
            // Using Chebyshev distance for 8 directional movement
            // Decision made to account for diagonal movement in grid
            return Math.Max(Math.Abs(x1 - x2), Math.Abs(y1 - y2));
        }
        /*!
         * \method ToString
         * \brief Returns a string representation of the world object.
         */
        public override string ToString()
        {
            return $"{{{nameof(MaxX)}={MaxX.ToString()}, {nameof(MaxY)}={MaxY.ToString()}, {nameof(_worldObjects)}={string.Join("," + _worldObjects)} }}";
        }


    }
}
