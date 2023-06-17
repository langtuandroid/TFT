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
        [Tooltip("Tiempo que tarda en recuperar magia")]
        private float _timeOfRecovering = .8f;
        [SerializeField]
        [Tooltip("Tiempo de recarga del poder máximo")]
        private float _timeToRechargeMaxPower = 10f;
        [SerializeField]
        [Tooltip("Duración del poder máximo en pantalla")]
        private float _maxPowerDuration = 5f;

        [Header("Stunning")]
        [SerializeField]
        [Tooltip("Tiempo que pasa el jugador aturdido")]
        private float _timeStunned = 5f;

        #endregion

        #region Public variables

        public bool IsDeath => _isDeath; // Indica si el player ha fallecido
        // Duración del poder máximo
        public float MaxPowerDuration => _maxPowerDuration;
        // Indica si está usando el poder máximo
        public bool IsUsingMaxPower => _isUsingMaxPower;
        // Indica si está aturdido
        public bool IsStunned => _stunnedTimer < _timeStunned;

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
        private SoulEvents _soulEvents; // Eventos de almas

        // Variables
        // Life
        private bool _isDeath; // Booleano que indica si el player ha decaído
        // Magic Attacks
        private float _magicTimer; // Temporizador para los ataques mágicos
        private float _maxPowerTimer; // Temporizador para la carga del ataque máximo 
        private bool _isUsingMaxPower; // Indica si se está usando el poder máximo
        private float _magicRecoverTimer; // Temporizador para recuperar magia
        // Stunning
        private float _stunnedTimer; // Tiempo aturdido

        #endregion

        #region Unity methods

        private void Awake()
        {
            _magicTimer = 0f;
            _magicRecoverTimer = 0f;
            _maxPowerTimer = _timeToRechargeMaxPower;
            _isUsingMaxPower = false;
            _stunnedTimer = _timeStunned;
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
            _magicEvents.OnUseOfMagicValue += OnUseOfMagicValue;
            // Souls
            _soulEvents = ServiceLocator.GetService<SoulEvents>();
            _soulEvents.OnGotSoulsValue += OnGotSouls;
        }

        private void Update()
        {
            if (IsStunned)
            {
                _stunnedTimer += Time.deltaTime;
                return;
            }

            if (_isDeath || _isUsingMaxPower)
                return;

            if (_magicTimer < _timeBetweenMagicAttacks)
                _magicTimer += Time.deltaTime;
            if (_maxPowerTimer < _timeToRechargeMaxPower)
                _maxPowerTimer += Time.deltaTime;

            // Recuperamos poder mágico
            RecoverMagic();
        }

        private void OnDestroy()
        {
            // Life
            _lifeEvents.OnHeartsValue -= OnIncrementMaxHealthValue;
            _lifeEvents.OnCurrentLifeValue -= OnCurrentHealthValue;
            _lifeEvents.OnDeathValue -= OnDeathValue;
            // Magic
            _magicEvents.OnMaxPowerUsedValue -= OnMaxPowerUsedValue;
            _magicEvents.OnMaxPowerFinalizedValue -= OnMaxPowerFinalizedValue;
            _magicEvents.OnUseOfMagicValue += OnUseOfMagicValue;
            // Souls
            _soulEvents.OnGotSoulsValue -= OnGotSouls;
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

        /// <summary>
        /// Indica que está usando el poder máximo
        /// </summary>
        /// <param name="time"></param>
        private void OnMaxPowerUsedValue(float time)
        {
            _isUsingMaxPower = true;
        }

        /// <summary>
        /// Indica que ya se ha finalizado el efecto del poder máximo
        /// </summary>
        private void OnMaxPowerFinalizedValue()
        {
            _isUsingMaxPower = false;
            _maxPowerTimer = 0;
        }

        /// <summary>
        /// Consume magia
        /// </summary>
        /// <param name="value"></param>
        private void OnUseOfMagicValue(int value)
        {
            int magic = CurrentMagic - value;
            CurrentMagic = Mathf.Max(0, magic);

            if (CurrentMagic == 0)
                _stunnedTimer = 0f;
        }

        /// <summary>
        /// Recupera la cantidad de magia que se usa
        /// </summary>
        private void RecoverMagic()
        {

            if (_magicRecoverTimer < _timeOfRecovering)
            {
                _magicRecoverTimer += Time.deltaTime;
                return;
            }

            CurrentMagic = Mathf.Min(MaxMagic, CurrentMagic + 1);
            _magicRecoverTimer = 0;
        }

        #endregion

        #region Souls

        private void OnGotSouls(int value)
        {
            CurrentSouls = Mathf.Min(MaxSouls, CurrentSouls + value);
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