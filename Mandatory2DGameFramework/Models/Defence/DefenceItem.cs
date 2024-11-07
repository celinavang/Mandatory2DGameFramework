using Mandatory2DGameFramework.Models.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.models.defence
{
    public class DefenceItem: IDefenceItem
    {
        public string Name { get; set; }
        public int ArmorClass { get; set; }
        public bool Lootable { get; set; }
        public bool Removeable { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }



        public DefenceItem(string name, int armorClass)
        {
            Name = name;
            ArmorClass = armorClass;
            Lootable = true;
            Removeable = true;
        }

        public override string ToString() { return $"Defence Item {Name} | AC: {ArmorClass}"; }
    }
}
