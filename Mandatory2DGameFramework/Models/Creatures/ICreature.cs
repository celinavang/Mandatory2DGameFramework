using Mandatory2DGameFramework.models.attack;
using Mandatory2DGameFramework.models.Creatures.States;
using Mandatory2DGameFramework.models.defence;
using Mandatory2DGameFramework.Model.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.models.Creatures
{
    public interface ICreature : IWorldObject
    {
        IAttackItem? Attack { get; set; }
        IDefenceItem? Defence { get; set; }
        int HitPoint { get; set; }
        int MaxHitPoints { get; set; }
        int ArmorClass { get; set; }
        int Strength { get; set; }
        int Dexterity { get; set; }
        bool Alive { get; set; }
        void Hit(ICreature enemy);
        void ReceiveHit(int toHit, int damage);
        void Loot(IWorldObject obj, World world);
        void DropItem();
        bool CanAttack(ICreature enemy, World world);
        void MoveTowardsObj(IWorldObject obj, World world);
        bool IsItemPreferred(IWorldObject obj);
        bool CanLoot(IWorldObject obj, World world);
        void TransitionTo(ICreatureState state);
    }
}
