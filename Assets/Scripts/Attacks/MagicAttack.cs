
using UnityEngine;

namespace Attack
{
    public abstract class MagicAttack : MonoBehaviour
    {

        /// <summary>
        /// Ataque débil
        /// </summary>
        public abstract void WeakAttack();

        /// <summary>
        /// Ataque medio
        /// </summary>
        public abstract void MediumAttack();

        public abstract void StopMediumAttack();

        /// <summary>
        /// Ataque fuerte
        /// </summary>
        public abstract void StrongAttack();

        /// <summary>
        /// Establece la dirección de los ataques
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="direction"></param>
        public abstract void SetDirection(Vector2 direction);
    }

}
