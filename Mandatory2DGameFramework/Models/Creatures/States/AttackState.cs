﻿using Mandatory2DGameFramework.Model.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.models.Creatures.States
{
    public class AttackState : ICreatureState
    {
        public bool Handle(ICreature creature, World world, IWorldObject obj)
        {
            creature.Hit(obj as Creature);
            return true;
        }
    }
}