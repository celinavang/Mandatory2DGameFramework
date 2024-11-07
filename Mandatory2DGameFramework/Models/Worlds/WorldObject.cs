namespace Mandatory2DGameFramework.Model.Worlds
{
    /*!
     * \class WorldObject
     * \brief Represents an object in the game world with properties like name, position, and lootability.
     * This class implements the IWorldObject interface, providing basic properties and methods for interaction.
     */
    public class WorldObject : IWorldObject
    {
        // Object's name
        public string Name { get; set; }
        // Whether the object can be looted
        public bool Lootable { get; set; }
        // Whether the object can be removed from the world
        public bool Removeable { get; set; }
        // X-coordinate position of the object

        public int PosX { get; set; }
        // Y-coordinate position of the object
        public int PosY { get; set; }

        /*!
         * \constructor WorldObject
         * \brief Initializes a new world object with specified properties.
         * \param name The name of the object.
         * \param lootable Whether the object can be looted.
         * \param removable Whether the object can be removed from the world.
         */
        public WorldObject(string name, bool lootable, bool removable)
        {
            Name = name;
            Lootable = lootable;
            Removeable = removable;
        }

        /*!
         * \method ToString
         * \brief Returns a string representation of the world object.
         */
        public override string ToString()
        {
            return $"{{{nameof(Name)}={Name}, {nameof(Lootable)}={Lootable.ToString()}, {nameof(Removeable)}={Removeable.ToString()}}}";
        }
    }
}
