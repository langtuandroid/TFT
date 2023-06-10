using UnityEngine;

namespace Player
{
    public class PlayerStatus : MonoBehaviour
    {

        #region SerializeField

        [Header("Data")]
        [SerializeField] private PlayerStatusSaveSO _playerStatusSaveSO;

        [Header("Magic Attack settings")]
        [SerializeField]
        [Tooltip("Tiempo entre ataques mágicos")]
        private float _timeBetweenMagicAttacks = .2f;
        [SerializeField]
        [Tooltip("Tiempo de recarga del poder máximo")]
        private float _timeToRechargeMaxPower = 10f;
        [SerializeField]
        [Tooltip("Duración del poder máximo en pantalla")]
        private float _maxPowerDuration = 5f;

        #endregion

        #region Public variables

        public bool IsDeath => _isDeath; // Indica si el player ha fallecido
        // Duración del poder máximo
        public float MaxPowerDuration => _maxPowerDuration;
        // Indica si está usando el poder máximo
        public bool IsUsingMaxPower => _isUsingMaxPower;

        public int CurrentHealth
        {
            get { return _playerStatusSaveSO.playerStatusSave.currentHealth; }
            set { _playerStatusSaveSO.playerStatusSave.currentHealth = value; }
        }
        public int MaxHealth
        {
            get { return _playerStatusSaveSO.playerStatusSave.maxHealth; }
            set { _playerStatusSaveSO.playerStatusSave.maxHealth = value; }
        }
        public int CurrentMagic
        {
            get { return _playerStatusSaveSO.playerStatusSave.currentMagic; }
            set { _playerStatusSaveSO.playerStatusSave.currentMagic = value; }
        }
        public int MaxMagic
        {
            get { return _playerStatusSaveSO.playerStatusSave.maxMagic; }
            set { _playerStatusSaveSO.playerStatusSave.maxMagic = value; }
        }
        public int CurrentSouls
        {
            get { return _playerStatusSaveSO.playerStatusSave.currentSouls; }
            set { _playerStatusSaveSO.playerStatusSave.currentSouls = value; }
        }
        public int MaxSouls
        {
            get { return _playerStatusSaveSO.playerStatusSave.maxSouls; }
            set { _playerStatusSaveSO.playerStatusSave.maxSouls = value; }
        }
        public int PrimarySkillIndex
        {
            get { return _playerStatusSaveSO.playerStatusSave.primarySkillIndex; }
            set { _playerStatusSaveSO.playerStatusSave.primarySkillIndex = value; }
        }
        public int SecondarySkillIndex
        {
            get { return _playerStatusSaveSO.playerStatusSave.secondarySkillIndex; }
            set { _playerStatusSaveSO.playerStatusSave.secondarySkillIndex = value; }
        }
        public bool IsJumpUnlocked
        {
            get { return _playerStatusSaveSO.playerStatusSave.isJumpUnlocked; }
            set { _playerStatusSaveSO.playerStatusSave.isJumpUnlocked = value; }
        }

        #endregion

        #region Private variables

        // Events
        private LifeEvents _lifeEvents; // Eventos de vida
        private MagicEvents _magicEvents; // Eventos de magia

        // Variables
        // Life
        private bool _isDeath; // Booleano que indica si el player ha decaído
        // Magic Attacks
        private float _magicTimer; // Temporizador para los ataques mágicos
        private float _maxPowerTimer; // Temporizador para la carga del ataque máximo 
        private bool _isUsingMaxPower; // Indica si se está usando el poder máximo

        #endregion

        #region Unity methods

        private void Awake()
        {
            _magicTimer = 0f;
            _maxPowerTimer = _timeToRechargeMaxPower;
            _isUsingMaxPower = false;
        }

        private void Start()
        {
            // EVENTS
            // Life
            _lifeEvents = ServiceLocator.GetService<LifeEvents>();
            _lifeEvents.OnHeartsValue += OnIncrementMaxHealthValue;
            _lifeEvents.OnCurrentLifeValue += OnCurrentHealthValue;
            _lifeEvents.OnDeathValue += OnDeathValue;
            // Magic
            _magicEvents = ServiceLocator.GetService<MagicEvents>();
            _magicEvents.OnMaxPowerUsedValue += OnMaxPowerUsedValue;
            _magicEvents.OnMaxPowerFinalizedValue += OnMaxPowerFinalizedValue;
            _magicEvents.DefineMaxPowerRechargingTime(_timeToRechargeMaxPower);
        }

        private void Update()
        {
            if (_magicTimer < _timeBetweenMagicAttacks)
                _magicTimer += Time.deltaTime;
            if (_maxPowerTimer < _timeToRechargeMaxPower)
                _maxPowerTimer += Time.deltaTime;

        }

        private void OnDestroy()
        {
            _lifeEvents.OnHeartsValue -= OnIncrementMaxHealthValue;
            _lifeEvents.OnCurrentLifeValue -= OnCurrentHealthValue;
            _lifeEvents.OnDeathValue -= OnDeathValue;
            _magicEvents.OnMaxPowerUsedValue -= OnMaxPowerUsedValue;
            _magicEvents.OnMaxPowerFinalizedValue -= OnMaxPowerFinalizedValue;
        }

        #endregion

        #region Public methods

        #region Magic attacks

        /// <summary>
        /// Devuelve true si está listo para usar el siguiente hechizo
        /// </summary>
        /// <returns></returns>
        public bool CanUseMagicAttacks()
        {
            return _magicTimer >= _timeBetweenMagicAttacks;
        }

        /// <summary>
        /// Devuelve true si el ataque máximo ya está recargado
        /// </summary>
        /// <returns></returns>
        public bool CanUseMaxPower()
        {
            return _maxPowerTimer >= _timeToRechargeMaxPower;
        }

        /// <summary>
        /// Reinicia el contador de tiempo
        /// </summary>
        public void RestartMagicTimer()
        {
            _magicTimer = 0f;
        }


        #endregion

        #endregion

        #region Private methods

        #region Health

        /// <summary>
        /// Aplica daño al jugador
        /// </summary>
        /// <param name="damage"></param>
        private void TakeDamage(int damage)
        {
            _lifeEvents.ChangeCurrentLifeQuantity(
                Mathf.Max(
                    0,
                    CurrentHealth - damage)
                );
        }

        /// <summary>
        /// Cura salud al jugador
        /// </summary>
        /// <param name="life"></param>
        private void HealLife(int life)
        {
            _lifeEvents.ChangeCurrentLifeQuantity(
                Mathf.Min(
                    MaxHealth,
                    CurrentHealth + life
                )
                );
        }

        /// <summary>
        /// Incrementa la cantidad máxima de salud
        /// (para cuando se consigue un corazón)
        /// </summary>
        private void OnIncrementMaxHealthValue()
        {
            MaxHealth += 2;
            _lifeEvents.ChangeCurrentLifeQuantity(MaxHealth);
        }

        /// <summary>
        /// Actualiza la salud actual
        /// </summary>
        /// <param name="value"></param>
        private void OnCurrentHealthValue(int value)
        {
            CurrentHealth = value;
        }

        /// <summary>
        /// Cambia el booleano para indicar 
        /// que fallece el personaje
        /// </summary>
        private void OnDeathValue()
        {
            _isDeath = true;
        }

        #endregion

        #region Magic Attacks

        private void OnMaxPowerUsedValue(float time)
        {
            _isUsingMaxPower = true;
        }

        private void OnMaxPowerFinalizedValue()
        {
            _isUsingMaxPower = false;
            _maxPowerTimer = 0;
        }

        #endregion


        #endregion

        #region Tests

        #region Health

        [ContextMenu("IncrementMaxHealthValue")]
        private void IncrementMaxHealthValue()
        {
            _lifeEvents.AddHeart();
        }

        [ContextMenu("Prueba de take damage")]
        private void TakeDamage()
        {
            //int value = Random.Range(1, 5);
            int value = 10;
            Debug.Log($"Voy a hacer {value} de daño");
            TakeDamage(value);
        }

        [ContextMenu("Prueba de heal life")]
        private void HealLife()
        {
            //int value = Random.Range(1, 5);
            int value = 10;
            Debug.Log($"Me curo {value} de salud");
            HealLife(value);
        }

        #endregion

        #endregion


    }
}