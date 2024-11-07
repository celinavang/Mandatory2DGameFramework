using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.Model.Worlds
{
    /*!
     * \interface IWorldObject
     * \brief Interface for objects that exist within the game world.
     * Objects that implement this interface have properties like position, name, and looting/removal capabilities.
     */
    public interface IWorldObject
    {
        /*!
         * \property Name
         * \brief Gets or sets the name of the world object.
         */
        string Name { get; set; }
        /*!
         * \property PosX
         * \brief Gets or sets the X-coordinate position of the object in the world.
         */
        int PosX { get; set; }
        /*!
         * \property PosY
         * \brief Gets or sets the Y-coordinate position of the object in the world.
         */
        int PosY { get; set; }
        /*!
         * \property Lootable
         * \brief Gets or sets whether the object can be looted by creatures.
         */
        bool Lootable { get; set; }
        /*!
         * \property Removeable
         * \brief Gets or sets whether the object can be removed from the world.
         */
        bool Removeable { get; set; }
        /*!
         * \method ToString
         * \brief Returns a string representation of the world object.
         */

        string ToString();
    }
}
