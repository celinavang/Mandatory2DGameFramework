using Mandatory2DGameFramework.models.attack;
using Mandatory2DGameFramework.models.Creatures;
using Mandatory2DGameFramework.Model.Worlds;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Mandatory2DGameFramework.models.defence;

namespace Mandatory2DGameFramework.Models.Scene
{
    public class WorldPrinter
    {
        private readonly World _world;

        public WorldPrinter(World world)
        {
            _world = world;
        }

        public void PrintWorld()
        {
            Console.Clear();
            string str = "";
            for (int i = 0; i < _world.MaxX+2; i++) 
            {
                str += "- ";
                
            }
            str += "\n";
            for (int i = 0; i < _world.MaxY; i++)
            {
                str += "\u001b[37m| ";
                for (int j = 0; j < _world.MaxX; j++)
                {
                    var obj = _world.GetObjectOnLocation(i, j);
                    if (obj != null)
                    {
                        if (obj is Creature)
                        {
                            str += "\u001b[37mC ";
                        }
                        else if (obj is WorldObject)
                        {
                            str += "\u001b[37m# ";
                        }
                        else if (obj is MeleeAttackItem || obj is RangedAttackItem)
                        {
                            str += "\u001b[37mA ";
                        }
                        else if (obj is DefenceItem)
                        {
                            str += "\u001b[37mD ";
                        }
                    }
                    else
                    {
                        str += "\u001b[40m\u001b[30m# ";
                    }

                }
                str += "\u001b[37m| \n";
            }
            for (int i = 0; i < _world.MaxX+2; i++)
            {
                str += "- ";

            }
            Console.WriteLine(str);
        }
    }
}
