
using UnityEngine;

namespace Attack
{
    public abstract class MagicAttack : MonoBehaviour
    {

        #region SerializeFields

        [Header("Attack costs")]
        [SerializeField]
        [Tooltip("Coste de los ataques (de menor a mayor poder)")]
        internal int[] _costs = new int[3];

        #endregion

        #region Internal variables

        // EVENTS
        internal MagicEvents _magicEvents;

        // STATES
        internal bool _isUsingWeakAttack;
        internal bool _isUsingMediumAttack;
        internal bool _isUsingStrongAttack;

        #endregion

        #region Public variables

        // EVENTS
        public bool IsUsingWeakAttack => _isUsingWeakAttack;
        public bool IsUsingMediumAttack => _isUsingMediumAttack;
        public bool IsUsingStrongAttack => _isUsingStrongAttack;

        #endregion


        #region Unity methods

        private void Awake()
        {
            // Eventos
            _magicEvents = ServiceLocator.GetService<MagicEvents>();

            // Variables
            _isUsingWeakAttack = false;
            _isUsingMediumAttack = false;
            _isUsingStrongAttack = false;
        }

        #endregion


        #region Abstract class methods

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

        /// <summary>
        /// Detiene el ataque medio
        /// </summary>
        public abstract void StopMediumAttack();

        /// <summary>
        /// Ataque fuerte
        /// </summary>
        public abstract void StrongAttack(Vector2 direction);

        #endregion

    }

}
