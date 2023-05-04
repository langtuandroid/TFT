
using System.Collections.Generic;
using UnityEngine;

namespace Attack
{
    public interface IAttack
    {

        /// <summary>
        /// Ataque débil
        /// </summary>
        void WeakAttack(GameObject prefab);

        /// <summary>
        /// Ataque medio
        /// </summary>
        void MediumAttack();

        /// <summary>
        /// Ataque fuerte
        /// </summary>
        void StrongAttack();

        /// <summary>
        /// Método para indicar que se pasa a ataque fuerte
        /// </summary>
        void ChangeStrongAttackState();

        /// <summary>
        /// Resetea los valores de las variables que contiene
        /// </summary>
        void ResetValues();

        void SetOriginAndDirection(Transform origin, Vector2 direction);
    }

}
