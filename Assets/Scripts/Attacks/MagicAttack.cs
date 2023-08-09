
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
        protected GameStatus _gameStatus;
        internal IAudioSpeaker _audioSpeaker;

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
            //_magicEvents = ServiceLocator.GetService<MagicEvents>();
            //_gameStatus = ServiceLocator.GetService<GameStatus>();

            // Variables
            _isUsingWeakAttack = false;
            _isUsingMediumAttack = false;
            _isUsingStrongAttack = false;
        }

        #endregion


        #region Abstract class methods

        /// <summary>
        /// Inicializa los eventos (para no tener que estar en cada clase
        /// consultando al ServiceLocator, solo se hace una vez desde el
        /// PlayerController)
        /// </summary>
        /// <param name="magicEvents"></param>
        /// <param name="gameStatus"></param>
        public virtual void Init(MagicEvents magicEvents, GameStatus gameStatus, IAudioSpeaker audioSpeaker)
        {
            _magicEvents = magicEvents;
            _gameStatus = gameStatus;
            _audioSpeaker = audioSpeaker;
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
        /// Detiene el ataque débil
        /// </summary>
        public virtual void StopWeakAttack() { }

        /// <summary>
        /// Ataque medio
        /// </summary>
        public abstract void MediumAttack(Vector2 direction);

        /// <summary>
        /// Detiene el ataque medio
        /// </summary>
        public virtual void StopMediumAttack() { }

        /// <summary>
        /// Ataque fuerte
        /// </summary>
        public abstract void StrongAttack(Vector2 direction);

        #endregion

    }

}
