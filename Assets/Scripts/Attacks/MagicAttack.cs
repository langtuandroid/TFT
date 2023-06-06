
using UnityEngine;

namespace Attack
{
    public abstract class MagicAttack : MonoBehaviour
    {

        private MagicEvents _magicEvents;

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
        /// Ataque débil
        /// </summary>
        public abstract void WeakAttack(Vector2 direction);

        /// <summary>
        /// Ataque medio
        /// </summary>
        public abstract void MediumAttack(Vector2 direction);

        public abstract void StopMediumAttack();

        /// <summary>
        /// Ataque fuerte
        /// </summary>
        public abstract void StrongAttack(Vector2 direction);

    }

}
