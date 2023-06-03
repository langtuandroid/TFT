using UnityEngine;

namespace Player
{
    public class PlayerStatus : MonoBehaviour
    {
        [SerializeField] private PlayerStatusSaveSO _playerStatusSaveSO;
        private LifeEvents _lifeEvents;

        private void Start()
        {
            _lifeEvents = ServiceLocator.GetService<LifeEvents>();
            _lifeEvents.OnHeartsValue += OnIncrementMaxHealthValue;
            _lifeEvents.OnCurrentLifeValue += OnCurrentHealthValue;
        }

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
        public bool IsJumpUnlocked
        {
            get { return _playerStatusSaveSO.playerStatusSave.isJumpUnlocked; }
            set { _playerStatusSaveSO.playerStatusSave.isJumpUnlocked = value; }
        }

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

        private void TakeDamage(int damage)
        {
            _lifeEvents.ChangeCurrentLifeQuantity(Mathf.Max(0, CurrentHealth - damage));
        }

        private void HealLife(int life)
        {
            _lifeEvents.ChangeCurrentLifeQuantity(Mathf.Min(MaxHealth, CurrentHealth + life));
        }

        private void OnIncrementMaxHealthValue()
        {
            MaxHealth += 2;
            _lifeEvents.ChangeCurrentLifeQuantity(MaxHealth);
        }

        private void OnCurrentHealthValue(int value)
        {
            CurrentHealth = value;
        }


    }
}