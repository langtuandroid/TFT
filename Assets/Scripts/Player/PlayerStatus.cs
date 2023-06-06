using UnityEngine;

namespace Player
{
    public class PlayerStatus : MonoBehaviour
    {

        #region SerializeField

        [SerializeField] private PlayerStatusSaveSO _playerStatusSaveSO;

        [SerializeField]
        [Tooltip("Tiempo entre ataques mágicos")]
        private float _timeBetweenMagicAttacks = .2f;

        #endregion

        #region Public variables

        public bool IsDeath => _isDeath;

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

        private LifeEvents _lifeEvents; // Eventos de vida
        private bool _isDeath; // Booleano que indica si el player ha decaído
        private float _magicTimer; // Temporizador para los ataques mágicos

        #endregion

        #region Unity methods

        private void Awake()
        {
            _magicTimer = 0f;
        }

        private void Start()
        {
            _lifeEvents = ServiceLocator.GetService<LifeEvents>();
            _lifeEvents.OnHeartsValue += OnIncrementMaxHealthValue;
            _lifeEvents.OnCurrentLifeValue += OnCurrentHealthValue;
            _lifeEvents.OnDeathValue += OnDeathValue;
        }

        private void Update()
        {
            if (_magicTimer < _timeBetweenMagicAttacks)
                _magicTimer += Time.deltaTime;
        }

        private void OnDestroy()
        {
            _lifeEvents.OnHeartsValue -= OnIncrementMaxHealthValue;
            _lifeEvents.OnCurrentLifeValue -= OnCurrentHealthValue;
            _lifeEvents.OnDeathValue -= OnDeathValue;
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