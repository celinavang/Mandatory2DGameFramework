using Mandatory2DGameFramework.model.attack;
using Mandatory2DGameFramework.model.defence;
using Mandatory2DGameFramework.worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.model.Creatures
{
    public class Creature
    {
        public string Name { get; set; }
        public int HitPoint { get; set; }
        public int ArmorClass { get; set; }
        public bool Alive { get; set; }

        // Todo consider how many attack / defence weapons are allowed
        public AttackItem?   Attack { get; set; }
        public DefenceItem?  Defence { get; set; }

        public int PosX { get; set; }
        public int PosY { get; set; }

        // Skills

        public int Strength { get; set; }
        public int Dexterity { get; set; }

        public Creature(string name, int hitPoint)
        {
            Name = name;
            HitPoint = 100;
            Alive = true;

            Attack = null;
            Defence = null;

        }

        public int Hit()
        {
            if (Attack != null)
            {
                if (Attack.DamageType == DamageTypes.strength)
                {
                    return Strength + Attack.Hit();
                }
                else if (Attack.DamageType == DamageTypes.dexterity)
                {
                    return Dexterity + Attack.Hit();
                }
            } 
            return 0;
        }

        public void ReceiveHit(int toHit, int damage)
        {
            int totalArmorClass = ArmorClass;
            if (Defence != null)
            {
                totalArmorClass += Defence.ReduceHitPoint;
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

        public void Loot(WorldObject obj)
        {
            if (obj.GetType() == typeof(AttackItem))
            {
                Attack = obj as AttackItem;
            }
            else if (obj.GetType() == typeof(DefenceItem))
            {
                Defence = obj as DefenceItem;
            }
        }

        public override string ToString()
        {
            return $"{{{nameof(Name)}={Name}, {nameof(HitPoint)}={HitPoint.ToString()}, {nameof(Attack)}={Attack}, {nameof(Defence)}={Defence}}}";
        }
    }
}
