
using UnityEngine;

namespace Attack
{
    public abstract class MagicAttack : MonoBehaviour
    {
        // Eventos
        internal MagicEvents _magicEvents;

        private void Awake()
        {
            _magicEvents = ServiceLocator.GetService<MagicEvents>();
        }

        /// <summary>
        /// Selecciona el tipo de ataque
        /// </summary>
        public virtual void Select()
        {
            _magicEvents.ChangeAttackType(this);
        }

        /// <summary>
        /// Deselecciona el elemento
        /// </summary>
        public abstract void DeSelect();

        /// <summary>
        /// Ataque débil
        /// </summary>
        public abstract void WeakAttack(Vector2 direction);

        /// <summary>
        /// Ataque medio
        /// </summary>
        public abstract void MediumAttack(Vector2 direction);

        /// <summary>
        /// Detiene el ataque medio
        /// </summary>
        public abstract void StopMediumAttack();

        /// <summary>
        /// Ataque fuerte
        /// </summary>
        public abstract void StrongAttack(Vector2 direction);

    }

}
