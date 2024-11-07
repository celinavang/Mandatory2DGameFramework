using Mandatory2DGameFramework.Model.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.models.defence
{
    public interface IDefenceItem : IWorldObject
    {
        int ArmorClass { get; set; }
    }
}
