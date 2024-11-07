using Mandatory2DGameFramework.Models.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.models.defence
{
    /*!
 * \class DefenceItem
 * \brief Represents a defensive item that provides armor class for defense.
 */
    public class DefenceItem: IDefenceItem
    {
        public string Name { get; set; }
        public int ArmorClass { get; set; }
        public bool Lootable { get; set; }
        public bool Removeable { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }

        /*!
 * \brief Constructor for a defensive item.
 * \param name The name of the defensive item.
 * \param armorClass The armor class provided by the item.
 */

        public DefenceItem(string name, int armorClass)
        {
            Name = name;
            ArmorClass = armorClass;
            Lootable = true;
            Removeable = true;
        }
        /*!
         * \brief Returns a string representation of the defence item.
         */
        public override string ToString() { return $"Defence Item {Name} | AC: {ArmorClass}"; }
    }
}
