using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class NewGameUI : MonoBehaviour
    {
        public static NewGameUI Instance { get; private set; }


        [Header("Load Game Buttons")]
        [SerializeField] private Button _firstSlotButton;
        [SerializeField] private Button _secondSlotButton;
        [SerializeField] private Button _thirdSlotButton;
        [SerializeField] private Button _returnButton;

        [Header("Confirmation Panel")]
        [SerializeField] private ConfirmationPanelUI _confirmationPanelUI;

        [Header("Save Data")]
        [SerializeField] private PlayerStatusSaveSO _playerStatusSaveSO;
        [SerializeField] private ZoneExitSideSO     _zoneExitSideSO;
        [SerializeField] private GameZoneSavesSO    _gameZoneSavesSO;

        private Button _lastSelectedButton;
        private GameSaveData[] _gameSaveDataArray = new GameSaveData[3];
        private int _slotToLoadIndex;


        private event Action OnReturnButtonClicked;


        private void Awake()
        {
            Instance = this;

            _returnButton.onClick.AddListener( () => Hide() );
            Hide();
        }


        public void Show( Action OnReturnButtonClicked )
        {
            gameObject.SetActive( true );
            ServiceLocator.GetService<GameInputs>().OnCancelPerformed += GameInputs_OnCancelPerformed;
            _firstSlotButton.Select();
            this.OnReturnButtonClicked = OnReturnButtonClicked;
        }

        public void Hide()
        {
            gameObject.SetActive( false );
            ServiceLocator.GetService<GameInputs>().OnCancelPerformed -= GameInputs_OnCancelPerformed;
            OnReturnButtonClicked?.Invoke();
        }

        private void GameInputs_OnCancelPerformed()
        {
            if ( gameObject.activeSelf )
                Hide();
        }
    }
}