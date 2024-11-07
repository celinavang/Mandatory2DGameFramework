using GameApplication;
using Mandatory2DGameFramework;
using Mandatory2DGameFramework.models.attack;
using Mandatory2DGameFramework.models.Creatures;
using System.Diagnostics;
using System;
using Mandatory2DGameFramework.models;
using System.ComponentModel;
using System.Threading;
using Mandatory2DGameFramework.Model.Worlds;
using Mandatory2DGameFramework.Models.Scene;

TryLog tryLog = new TryLog();
tryLog.Start();

World world = new World(10, 10);
Creature c1 = new Creature("C1", 100);
world.AddOnLocation(1, 1, c1);
Creature c2 = new Creature("C2", 100);
world.AddOnLocation(5, 7, c2);
WorldObject WO1 = new WorldObject("WO1", false, false);
world.AddOnLocation(3, 3, WO1);
WorldObject WO3 = new WorldObject("WO3", false, false);
world.AddOnLocation(3, 4, WO3);
WorldObject WO4 = new WorldObject("WO4", false, false);
world.AddOnLocation(3, 5, WO4);
WorldObject WO5 = new WorldObject("WO5", false, false);
world.AddOnLocation(3, 2, WO5);
WorldObject WO6 = new WorldObject("WO6", false, false);
world.AddOnLocation(3, 1, WO6);
WorldObject WO7 = new WorldObject("WO7", false, false);
world.AddOnLocation(3, 0, WO7);

IAttackItem WO2 = AttackItemFactory.CreateAttackItem(AttackType.melee, "Sword", 10);
world.AddOnLocation(1, 2, WO2);
WorldPrinter worldPrinter = new WorldPrinter(world);
CreatureObserver creatureObserver = new CreatureObserver();
creatureObserver.SubscribeToCreature(c1);
creatureObserver.SubscribeToCreature(c2);


worldPrinter.PrintWorld();
Thread.Sleep(1000);
while (c2.Alive)
{
    c1.TakeTurn(world);
    worldPrinter.PrintWorld();
    Thread.Sleep(1000); 
    c2.TakeTurn(world);
    worldPrinter.PrintWorld();
    Thread.Sleep(1000);
}



