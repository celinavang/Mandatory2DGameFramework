using Mandatory2DGameFramework.Model.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.models.attack
{
    public interface IAttackItem : IWorldObject
    {
        DamageType DamageType { get; set; }
        AttackType AttackType { get; set; }
        public int MaxDamage { get; set; }
    }
}
