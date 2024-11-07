using Mandatory2DGameFramework.Models;
using System.Diagnostics;


namespace Mandatory2DGameFramework.models.attack
{
    /*!
* \brief Factory class for creating attack items.
* 
* Provides a static method for creating instances of \c IAttackItem based on the specified attack type. 
* Uses the \c AttackType enumeration to determine the type of item to create (melee or ranged).
*/
    public class AttackItemFactory
    {
        /*!
* \brief Creates an attack item of a specified type.
* 
* This method creates and returns a melee or ranged attack item based on the provided \c AttackType.
* Logs the creation of each item type and throws an exception if an invalid attack type is provided.
*
* \param attackType The type of attack item to create. This can be either \c AttackType.melee or \c AttackType.ranged.
* \param name The name of the attack item.
* \param maxDamage The maximum damage value for the attack item.
* 
* \return An instance of \c IAttackItem corresponding to the specified attack type.
* 
* \throws Exception If an invalid attack type is provided.
* 
* \note This factory method uses tracing to log information about the item creation.
*/

        public static IAttackItem CreateAttackItem(AttackType attackType, string name, int maxDamage)
        {

            switch (attackType)
            {
                case AttackType.melee:
                    MyLogger.TraceInformation("Creating melee attack item:");
                    return new MeleeAttackItem(name, maxDamage);
                case AttackType.ranged:
                    MyLogger.TraceInformation("Creating ranged attack item");
                    return new RangedAttackItem(name, maxDamage);
                default:
                    MyLogger.TraceError("Could not create Attack Item - Invalid attack type");
                    throw new Exception("Invalid attack type");
            }

        }
    }
}
