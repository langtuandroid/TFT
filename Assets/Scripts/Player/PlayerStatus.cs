using UnityEngine;

namespace Player
{
    public class PlayerStatus
    {

        #region SerializeField

        [Header("Data")]
        private PlayerStatusSaveSO _playerStatusSaveSO;
        private PlayerStatusSettingDataSO _playerStatusSettingDataSO;

        #endregion

        #region Public variables

        // PLAYER STATES
        // Indica si el player ha fallecido
        public bool IsDeath => _isDeath;
        // Indica si el player tiene invencibilidad temporal
        public bool HasTemporalInvencibility => _hasTemporalInvencibility;
        // Indica si est� aturdido
        public bool IsStunned => _stunnedTimer < _playerStatusSettingDataSO.TimeStunned;

        // MAGIC ATTACK VARIABLES
        // Duraci�n del poder m�ximo
        public float MaxPowerDuration => _playerStatusSettingDataSO.MaxPowerDuration;

        // GENERAL VARIABLES
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

        public bool IsWeakMagicUnlocked
        {
            get
            {
                switch (PrimarySkillIndex)
                {
                    case 0:
                        return _playerStatusSaveSO.playerStatusSave.isFireWeakUnlocked;
                    case 1:
                        return _playerStatusSaveSO.playerStatusSave.isPlantWeakUnlocked;
                    case 2:
                        return _playerStatusSaveSO.playerStatusSave.isWaterWeakUnlocked;
                    default:
                        return false;
                }

            }
        }

        public bool IsMediumMagicUnlocked
        {
            get
            {
                switch (PrimarySkillIndex)
                {
                    case 0:
                        return _playerStatusSaveSO.playerStatusSave.isFireMediumUnlocked;
                    case 1:
                        return _playerStatusSaveSO.playerStatusSave.isPlantMediumUnlocked;
                    case 2:
                        return _playerStatusSaveSO.playerStatusSave.isWaterMediumUnlocked;
                    default:
                        return false;
                }

            }
        }

        public bool IsStrongMagicUnlocked
        {
            get
            {
                switch (PrimarySkillIndex)
                {
                    case 0:
                        return _playerStatusSaveSO.playerStatusSave.isFireStrongUnlocked;
                    case 1:
                        return _playerStatusSaveSO.playerStatusSave.isPlantStrongUnlocked;
                    case 2:
                        return _playerStatusSaveSO.playerStatusSave.isWaterStrongUnlocked;
                    default:
                        return false;
                }

            }
        }

        public bool CanUseSecondarySkill
        {
            get
            {
                switch (SecondarySkillIndex)
                {
                    case 0:
                        return _playerStatusSaveSO.playerStatusSave.isLightUnlocked;
                    case 1:
                        return _playerStatusSaveSO.playerStatusSave.isAirUnlocked;
                    case 2:
                        return _playerStatusSaveSO.playerStatusSave.isHeavyMovementUnlocked;
                    case 3:
                        return _playerStatusSaveSO.playerStatusSave.lifeBerryUnlocked && _playerStatusSaveSO.playerStatusSave.lifeBerryQuantity > 0;
                    case 4:
                        return _playerStatusSaveSO.playerStatusSave.magicBerryUnlocked && _playerStatusSaveSO.playerStatusSave.magicBerryQuantity > 0;
                    case 5:
                        return _playerStatusSaveSO.playerStatusSave.bombBerryUnlocked && _playerStatusSaveSO.playerStatusSave.bombBerryQuantity > 0;
                    default:
                        return false;
                }
            }
        }

        #endregion

        #region Private variables

        // Data

        // Events
        private LifeEvents _lifeEvents; // Eventos de vida
        private MagicEvents _magicEvents; // Eventos de magia
        private SoulEvents _soulEvents; // Eventos de almas
        private GameStatus _gameStatus; // Estado de juego

        // Variables
        // Life
        private bool _isDeath; // Booleano que indica si el player ha deca�do
        private bool _hasTemporalInvencibility; // Booleano que indica si el player est� bajo efecto de invencibilidad temporal
        // Magic Attacks
        private float _magicTimer; // Temporizador para los ataques m�gicos
        private float _maxPowerTimer; // Temporizador para la carga del ataque m�ximo 
        private bool _isUsingMaxPower; // Indica si se est� usando el poder m�ximo
        private float _magicRecoverTimer; // Temporizador para recuperar magia
        // Stunning
        private float _stunnedTimer;

        #endregion

        #region Constructor

        public PlayerStatus(PlayerStatusSaveSO playerStatusSaveSO, PlayerStatusSettingDataSO playerStatusSettingDataSO)
        {
            // Data
            _playerStatusSaveSO = playerStatusSaveSO;
            _playerStatusSettingDataSO = playerStatusSettingDataSO;
            // Magic attacks
            _magicTimer = 0f;
            _magicRecoverTimer = 0f;
            _maxPowerTimer = _playerStatusSettingDataSO.TimeToRechargeMaxPower;
            _isUsingMaxPower = false;
            _stunnedTimer = _playerStatusSettingDataSO.TimeStunned;
        }

        #endregion

        #region Public methods

        #region Initialization

        public void Init(
            LifeEvents lifeEvents,
            MagicEvents magicEvents,
            SoulEvents soulEvents,
            GameStatus gameStatus
            )
        {
            // Life
            _lifeEvents = lifeEvents;
            _lifeEvents.OnHeartsValue += OnIncrementMaxHealthValue;
            _lifeEvents.OnCurrentLifeValue += OnCurrentHealthValue;
            _lifeEvents.OnDeathValue += OnDeathValue;
            _lifeEvents.OnStopTemporalInvencibility += OnStopInvencibilityValue;


            // Magic attacks
            _magicEvents = magicEvents;
            _magicEvents.OnMaxPowerUsedValue += OnMaxPowerUsedValue;
            _magicEvents.OnMaxPowerFinalizedValue += OnMaxPowerFinalizedValue;
            _magicEvents.DefineMaxPowerRechargingTime(_playerStatusSettingDataSO.TimeToRechargeMaxPower);
            _magicEvents.OnUseOfMagicValue += OnUseOfMagicValue;

            // Souls
            _soulEvents = soulEvents;
            _soulEvents.OnGotSoulsValue += OnGotSouls;

            // GameStatus
            _gameStatus = gameStatus;
        }

        public void DestroyElements()
        {
            // Life
            _lifeEvents.OnHeartsValue -= OnIncrementMaxHealthValue;
            _lifeEvents.OnCurrentLifeValue -= OnCurrentHealthValue;
            _lifeEvents.OnDeathValue -= OnDeathValue;
            _lifeEvents.OnStopTemporalInvencibility -= OnStopInvencibilityValue;
            // Magic
            _magicEvents.OnMaxPowerUsedValue -= OnMaxPowerUsedValue;
            _magicEvents.OnMaxPowerFinalizedValue -= OnMaxPowerFinalizedValue;
            _magicEvents.OnUseOfMagicValue -= OnUseOfMagicValue;
            // Souls
            _soulEvents.OnGotSoulsValue -= OnGotSouls;
        }

        public void UpdateInfo()
        {
            if (IsStunned)
            {
                _stunnedTimer += Time.deltaTime;
                return;
            }

            if (_isDeath || _isUsingMaxPower)
                return;

            if (_magicTimer < _playerStatusSettingDataSO.TimeBetweenMagicAttacks)
                _magicTimer += Time.deltaTime;
            if (_maxPowerTimer < _playerStatusSettingDataSO.TimeToRechargeMaxPower)
                _maxPowerTimer += Time.deltaTime;

            // Recuperamos poder m�gico
            RecoverMagic();
        }

        #endregion

        #region Magic attacks

        /// <summary>
        /// Devuelve true si est� listo para usar el siguiente hechizo
        /// </summary>
        /// <returns></returns>
        public bool CanUseMagicAttacks()
        {
            return _magicTimer >= _playerStatusSettingDataSO.TimeBetweenMagicAttacks;
        }

        /// <summary>
        /// Devuelve true si el ataque m�ximo ya est� recargado
        /// </summary>
        /// <returns></returns>
        public bool CanUseMaxPower()
        {
            return _maxPowerTimer >= _playerStatusSettingDataSO.TimeToRechargeMaxPower;
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
        /// Aplica da�o al jugador
        /// </summary>
        /// <param name="damage"></param>
        public void TakeDamage(int damage)
        {
            if (_hasTemporalInvencibility)
                return;

            // Activamos el evento de recibir da�o
            _lifeEvents.ChangeCurrentLifeQuantity(
                Mathf.Max(
                    0,
                    CurrentHealth - damage)
                );

            // Y aplicamos invencibilidad temporal
            StartTemporalInvencibility();

            Debug.Log("Le he quitado daño");
        }

        private void StartTemporalInvencibility()
        {
            _hasTemporalInvencibility = true;
            _lifeEvents.StartTemporalInvencibility(_playerStatusSettingDataSO.TimeOfInvencibility);
        }

        private void OnStopInvencibilityValue()
        {
            _hasTemporalInvencibility = false;
        }

        /// <summary>
        /// Cura salud al jugador
        /// </summary>
        /// <param name="life"></param>
        public void HealLife(int life)
        {
            _lifeEvents.ChangeCurrentLifeQuantity(
                Mathf.Min(
                    MaxHealth,
                    CurrentHealth + life
                )
                );
        }

        /// <summary>
        /// Incrementa la cantidad m�xima de salud
        /// (para cuando se consigue un coraz�n)
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
            _gameStatus.AskChangeToInactiveState();
        }

        #endregion

        #region Magic Attacks

        /// <summary>
        /// Indica que est� usando el poder m�ximo
        /// </summary>
        /// <param name="time"></param>
        private void OnMaxPowerUsedValue(float time)
        {
            _isUsingMaxPower = true;
        }

        /// <summary>
        /// Indica que ya se ha finalizado el efecto del poder m�ximo
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

            if (_magicRecoverTimer < _playerStatusSettingDataSO.TimeOfRecovering)
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

    }
}