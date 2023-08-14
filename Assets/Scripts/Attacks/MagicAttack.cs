
using UnityEngine;
using Player;

namespace Attack
{
    public abstract class MagicAttack
    {

        #region Internal variables

        // DATA
        internal MagicAttackSettingsSO _magicSettingsSO;
        internal Transform _transform;

        // EVENTS
        internal MagicEvents _magicEvents;
        protected GameStatus _gameStatus;
        internal IAudioSpeaker _audioSpeaker;
        internal PlayerStatus _playerStatus;

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


        #region Class constructor

        public MagicAttack()
        {
            // Eventos
            //_magicEvents = ServiceLocator.GetService<MagicEvents>();
            //_gameStatus = ServiceLocator.GetService<GameStatus>();

            // Variables de estado
            Initialize();
        }

        #endregion


        #region Abstract class methods

        /// <summary>
        /// Inicializa las variables de estado para cada tipo de ataque
        /// </summary>
        internal void Initialize()
        {
            _isUsingWeakAttack = false;
            _isUsingMediumAttack = false;
            _isUsingStrongAttack = false;
        }

        /// <summary>
        /// Inicializa los eventos (para no tener que estar en cada clase
        /// consultando al ServiceLocator, solo se hace una vez desde el
        /// PlayerController)
        /// </summary>
        /// <param name="magicEvents"></param>
        /// <param name="gameStatus"></param>
        public virtual void Init(MagicAttackSettingsSO magicSettings, PlayerStatus playerStatus, MagicEvents magicEvents, GameStatus gameStatus, IAudioSpeaker audioSpeaker, Transform transform)
        {
            _transform = transform;
            _magicSettingsSO = magicSettings;
            _playerStatus = playerStatus;
            _magicEvents = magicEvents;
            _gameStatus = gameStatus;
            _audioSpeaker = audioSpeaker;
        }

        /// <summary>
        /// Para comprobaciones como si fuera Update
        /// </summary>
        public abstract void Run(Vector2 direction);      

        /// <summary>
        /// Para destruir el elemento
        /// </summary>
        public abstract void Destroy();

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
