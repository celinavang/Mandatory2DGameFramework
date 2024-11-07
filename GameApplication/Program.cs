using Mandatory2DGameFramework.models.attack;
using Mandatory2DGameFramework.models.Creatures;
using Mandatory2DGameFramework.Model.Worlds;
using Mandatory2DGameFramework.Models.Scene;
using Mandatory2DGameFramework.Models;
using System.Xml;
using System.Reflection.Emit;
using Mandatory2DGameFramework.models.defence;

MyLogger logger = new MyLogger();
logger.Start();

XmlDocument configDoc = new XmlDocument();
configDoc.Load("C:/Users/civah/OneDrive/Code/Mandatory2DGameFramework/GameApplication/App.xml");

if (configDoc.DocumentElement == null)
{
    Console.WriteLine("No root element found in config file");
    return;
}

XmlNodeList creatures = configDoc.DocumentElement.SelectNodes("creatures/creature");
XmlNodeList MeleeWeapons = configDoc.DocumentElement.SelectNodes("weapons/melee/weapon");
XmlNodeList RangedWeapons = configDoc.DocumentElement.SelectNodes("weapons/ranged/weapon");
XmlNodeList DefenceItems = configDoc.DocumentElement.SelectNodes("defenceItems/defence");
XmlNodeList StaticItems = configDoc.DocumentElement.SelectNodes("staticItems/item");



World world = new World(10, 10);
CreatureObserver creatureObserver = new CreatureObserver(world);

for (int i = 0; i < 3; i++)
{
    foreach (XmlNode node in creatures)
    {
        Random random = new Random();
        Creature creature = new Creature(node["name"].InnerText, int.Parse(node["health"].InnerText), int.Parse(node["armorclass"].InnerText), int.Parse(node["strength"].InnerText), int.Parse(node["dexterity"].InnerText));
        creatureObserver.SubscribeToCreature(creature);
        Tuple<int, int> pos = new Tuple<int, int>(0, 0);
        bool foundPos = false;
        while (!foundPos)
        {
            pos = new Tuple<int, int>(random.Next(1, world.MaxX), random.Next(1, world.MaxY));
            foundPos = world.LocationFree(pos.Item1, pos.Item2);
        }
        world.AddOnLocation(pos.Item1, pos.Item2, creature);
        Console.WriteLine(creature.ToString());
    }
}


foreach (XmlNode node in MeleeWeapons)
{
    Random random = new Random();
    IAttackItem weapon = AttackItemFactory.CreateAttackItem(AttackType.melee, node["name"].InnerText, int.Parse(node["damage"].InnerText));
    Tuple<int, int> pos = new Tuple<int, int>(0, 0);
    bool foundPos = false;
    while (!foundPos)
    {
        pos = new Tuple<int, int>(random.Next(1, world.MaxX), random.Next(1, world.MaxY));
        foundPos = world.LocationFree(pos.Item1, pos.Item2);
    }
    world.AddOnLocation(pos.Item1, pos.Item2, weapon);
    Console.WriteLine(weapon.ToString());
}
foreach (XmlNode node in RangedWeapons)
{
    Random random = new Random();
    IAttackItem weapon = AttackItemFactory.CreateAttackItem(AttackType.melee, node["name"].InnerText, int.Parse(node["damage"].InnerText));
    Tuple<int, int> pos = new Tuple<int, int>(0, 0);
    bool foundPos = false;
    while (!foundPos)
    {
        pos = new Tuple<int, int>(random.Next(1, world.MaxX), random.Next(1, world.MaxY));
        foundPos = world.LocationFree(pos.Item1, pos.Item2);
    }
    world.AddOnLocation(pos.Item1, pos.Item2, weapon);
    Console.WriteLine(weapon.ToString());
}

foreach (XmlNode node in DefenceItems)
{
    Random random = new Random();
    IDefenceItem defenceItem = new DefenceItem(node["name"].InnerText, int.Parse(node["armorclass"].InnerText));
    Tuple<int, int> pos = new Tuple<int, int>(0, 0);
    bool foundPos = false;
    while (!foundPos)
    {
        pos = new Tuple<int, int>(random.Next(1, world.MaxX), random.Next(1, world.MaxY));
        foundPos = world.LocationFree(pos.Item1, pos.Item2);
    }
    world.AddOnLocation(pos.Item1, pos.Item2, defenceItem);
    Console.WriteLine(defenceItem.ToString());
}
for (int i = 0; i < 3; i++)
{
    foreach (XmlElement node in StaticItems)
    {
        Random random = new Random();
        WorldObject worldObject = new WorldObject(node["name"].InnerText, false, false);
        Tuple<int, int> pos = new Tuple<int, int>(0, 0);
        bool foundPos = false;
        while (!foundPos)
        {
            pos = new Tuple<int, int>(random.Next(1, world.MaxX), random.Next(1, world.MaxY));
            foundPos = world.LocationFree(pos.Item1, pos.Item2);
        }
        world.AddOnLocation(pos.Item1, pos.Item2, worldObject);
        Console.WriteLine(worldObject.ToString());

    }
}


    WorldPrinter worldPrinter = new WorldPrinter(world);
worldPrinter.PrintWorld();
Thread.Sleep(1000);

while (world._creatures.Count() > 1)
{
    foreach (ICreature creature in world._creatures.ToList())
    {
        if (creature.Alive)
        {
            creature.TakeTurn(world);
            worldPrinter.PrintWorld();
            Thread.Sleep(50);
        }
    }
}
