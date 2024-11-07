using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Model.Worlds
{
    public class WorldObject : IWorldObject
    {
        public string Name { get; set; }
        public bool Lootable { get; set; }
        public bool Removeable { get; set; }

        public int PosX { get; set; }
        public int PosY { get; set; }

        public WorldObject(string name, bool lootable, bool removable)
        {
            Name = name;
            Lootable = lootable;
            Removeable = removable;
        }

        public override string ToString()
        {
            return $"{{{nameof(Name)}={Name}, {nameof(Lootable)}={Lootable.ToString()}, {nameof(Removeable)}={Removeable.ToString()}}}";
        }
    }
}
