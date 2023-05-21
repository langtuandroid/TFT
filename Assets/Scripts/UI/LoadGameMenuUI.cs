// ************ @autor: Álvaro Repiso Romero *************
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class LoadGameMenuUI : MonoBehaviour
    {
        public static LoadGameMenuUI Instance { get; private set; }


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
        [SerializeField] private ZoneSaveSO[]       _zoneSaveSOArray;

        private GameSaveData[] _gameSaveDataArray = new GameSaveData[3];
        private int _slotToLoadIndex;


        private event Action OnReturnButtonClicked;


        private void Awake()
        {
            Instance = this;
            SetSaveSlotInfoAndButtonsEvent();

            _returnButton.onClick.AddListener( () => Hide() );
            Hide();
        }

        private void SetSaveSlotInfoAndButtonsEvent()
        {
            SaveGame saveGame = new SaveGame();

            _gameSaveDataArray[0] = saveGame.LoadGameSaveData( 1 );
            if ( _gameSaveDataArray[0] != null )
            {
                _firstSlotButton.onClick.AddListener( () => OpenConfirmationPanel( 0 ) );
                _firstSlotButton.GetComponentInChildren<TextMeshProUGUI>().text  = "Save Game 1";
            }

            _gameSaveDataArray[1] = saveGame.LoadGameSaveData( 2 );
            if ( _gameSaveDataArray[1] != null )
            {
                _secondSlotButton.onClick.AddListener( () => OpenConfirmationPanel( 1 ) );
                _secondSlotButton.GetComponentInChildren<TextMeshProUGUI>().text = "Save Game 2";
            }

            _gameSaveDataArray[2] = saveGame.LoadGameSaveData( 3 );
            if ( _gameSaveDataArray[2] != null )
            {
                _thirdSlotButton.onClick.AddListener( () => OpenConfirmationPanel( 2 ) );
                _thirdSlotButton.GetComponentInChildren<TextMeshProUGUI>().text  = "Save Game 3";
            }
        }

        private void OpenConfirmationPanel( int saveSlot )
        {
            _slotToLoadIndex = saveSlot;
            _confirmationPanelUI.Show( LoadGame );
        }

        private void LoadGame()
        {
            _playerStatusSaveSO.playerStatusSave = _gameSaveDataArray[_slotToLoadIndex].playerStatusSave;
            _zoneExitSideSO.nextStartPointRefID  = _gameSaveDataArray[_slotToLoadIndex].startPointRefID;

            _playerStatusSaveSO.playerStatusSave = _gameSaveDataArray[_slotToLoadIndex].playerStatusSave;

            int arrayLength = _gameSaveDataArray[_slotToLoadIndex].zoneSavesArray.Length;
            for ( int i = 0; i < arrayLength; i++ )
                _zoneSaveSOArray[i].zoneSave = _gameSaveDataArray[_slotToLoadIndex].zoneSavesArray[i];

            Debug.Log( "Loaded" );
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