using Mandatory2DGameFramework.model.Creatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.worlds
{
    public class World
    {
        public int MaxX { get; set; }
        public int MaxY { get; set; }


        // world objects
        private List<WorldObject> _worldObjects;
        // world creatures
        private List<Creature> _creatures;

        public World(int maxX, int maxY)
        {
            MaxX = maxX;
            MaxY = maxY;
            _worldObjects = new List<WorldObject>();
            _creatures = new List<Creature>();
        }

        public bool LocationFree(int x, int y)
        {
            foreach (var worldObject in _worldObjects)
            {
                if (worldObject.PosX == x && worldObject.PosY == y)
                {
                    return false;
                }
            }
            foreach (var creature in _creatures)
            {
                if (creature.PosX == x && creature.PosY == y)
                {
                    return false;

                }
            } 
            return true;
        }

        public override string ToString()
        {
            return $"{{{nameof(MaxX)}={MaxX.ToString()}, {nameof(MaxY)}={MaxY.ToString()}}}";
        }
    }
}
