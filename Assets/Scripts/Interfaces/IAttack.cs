
using System;
using UnityEngine;

namespace Attack
{
    public interface IAttack
    {

        /// <summary>
        /// Ataque d�bil
        /// </summary>
        void WeakAttack(GameObject prefab);

        /// <summary>
        /// Ataque medio
        /// </summary>
        void MediumAttack();

        /// <summary>
        /// Ataque fuerte
        /// </summary>
        void StrongAttack(System.Object element);

        /// <summary>
        /// Establece el origen y la direcci�n de los ataques
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="direction"></param>
        void SetOriginAndDirection(Transform origin, Vector2 direction);
    }

}
