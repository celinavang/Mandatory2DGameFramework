using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandatory2DGameFramework.models.attack
{
    /*!
* \enum DamageType
* \brief Specifies the different types of damagetypes that can be used by creatures or items.
*/
    public enum DamageType
    {
        /*! 
         * \brief Strength-based damage.*/
        strength,
        /*! 
         * \brief Dexterity-based damage.*/
        dexterity
    }
}
