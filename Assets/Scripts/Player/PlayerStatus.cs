using UnityEngine;
using DG.Tweening;
using UnityEditor;
using System;
using Attack;

namespace Player
{
    public class PlayerStatus : MonoBehaviour
    {

        #region SerializeField

        [Header("Data")]
        [SerializeField] private PlayerStatusSaveSO _playerStatusSaveSO;

        [Header("Magic Attack settings")]
        [SerializeField]
        [Tooltip("Tiempo entre ataques m�gicos")]
        private float _timeBetweenMagicAttacks = .2f;
        [SerializeField]
        [Tooltip("Tiempo que tarda en recuperar magia")]
        private float _timeOfRecovering = .8f;
        [SerializeField]
        [Tooltip("Tiempo de recarga del poder m�ximo")]
        private float _timeToRechargeMaxPower = 10f;
        [SerializeField]
        [Tooltip("Duraci�n del poder m�ximo en pantalla")]
        private float _maxPowerDuration = 5f;

        [Header("Life & Health settings")]
        [SerializeField]
        [Tooltip("Sprite del personaje")]
        private SpriteRenderer _playerSprite;
        [SerializeField]
        [Tooltip("Tiempo que dura la invencibilidad tras recibir da�o")]
        private float _timeOfInvencibility = 2.5f;

        [Header("Stunning")]
        [SerializeField]
        [Tooltip("Tiempo que pasa el jugador aturdido")]
        private float _timeStunned = 5f;

        #endregion

        #region Public variables

        // PLAYER STATES
        // Indica si el player ha fallecido
        public bool IsDeath => _isDeath;
        // Indica si el player tiene invencibilidad temporal
        public bool HasTemporalInvencibility => _hasTemporalInvencibility;
        // Indica si est� aturdido
        public bool IsStunned => _stunnedTimer < _timeStunned;

        // MAGIC ATTACK VARIABLES
        // Duraci�n del poder m�ximo
        public float MaxPowerDuration => _maxPowerDuration;

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

        #endregion

        #region Private variables

        // Events
        private LifeEvents _lifeEvents; // Eventos de vida
        private MagicEvents _magicEvents; // Eventos de magia
        private SoulEvents _soulEvents; // Eventos de almas

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

            // Recuperamos poder m�gico
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
            _magicEvents.OnUseOfMagicValue -= OnUseOfMagicValue;
            // Souls
            _soulEvents.OnGotSoulsValue -= OnGotSouls;
        }

        #endregion

        #region Public methods

        #region Magic attacks

        /// <summary>
        /// Devuelve true si est� listo para usar el siguiente hechizo
        /// </summary>
        /// <returns></returns>
        public bool CanUseMagicAttacks()
        {
            return _magicTimer >= _timeBetweenMagicAttacks;
        }

        /// <summary>
        /// Devuelve true si el ataque m�ximo ya est� recargado
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
            tween = GetTemporalInvencibility();
        }

        private Tween tween;
        private Tween GetTemporalInvencibility()
        {
            // Activamos la invencibilidad temporal
            _hasTemporalInvencibility = true;
            Sequence seq = DOTween.Sequence();

            seq.Append(_playerSprite.DOFade(60 / 255f, 0f));
            seq.Append(_playerSprite.DOFade(1f, _timeOfInvencibility))
                .SetEase(Ease.InOutFlash, 14, -1)
                ;

            seq.OnComplete(() => _hasTemporalInvencibility = false);
            seq.Play();

            return seq;
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
            if (tween != null)
                tween.Kill();

            _playerSprite.DOFade(1f, 0f).Play();
            _isDeath = true;

            Invoke(nameof(ReturnToMainMenu), 5f);
        }

        private void ReturnToMainMenu()
        {
            ServiceLocator.GetService<IAudioSpeaker>().ChangeMusic(MusicName.Main_Menu);
            ServiceLocator.GetService<SceneLoader>().Load(SceneName.S00_MainMenuScene.ToString());
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
            int value = 1;
            Debug.Log($"Voy a hacer {value} de da�o");
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