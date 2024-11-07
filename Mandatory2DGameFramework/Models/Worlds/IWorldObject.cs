using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Model.Worlds
{
    public interface IWorldObject
    {
        string Name { get; set; }
        int PosX { get; set; }
        int PosY { get; set; }
        bool Lootable { get; set; }
        bool Removeable { get; set; }

        string ToString();
    }
}
