using Mandatory2DGameFramework.Model.Worlds;

namespace Mandatory2DGameFramework.models.defence
{
    /*!
 * \interface IDefenceItem
 * \brief Interface for defensive items, which provide armor class to a creature.
 */
    public interface IDefenceItem : IWorldObject
    {
        /*!
         * \property ArmorClass
         * \brief Gets or sets the armor class provided by the defence item.
         */
        int ArmorClass { get; set; }
    }
}
