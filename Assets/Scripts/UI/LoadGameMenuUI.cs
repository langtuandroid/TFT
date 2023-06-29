// ************ @autor: �lvaro Repiso Romero *************
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class LoadGameMenuUI : MonoBehaviour
    {
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
        private string _loadMessage = "You want to load ";

        private event Action OnReturnButtonClicked;


        private void Awake()
        {
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
                _firstSlotButton.onClick.AddListener( () => {
                    _lastSelectedButton = _firstSlotButton;
                    OpenLoadConfirmationPanel( 0 );
                    } );

                _firstSlotButton.GetComponentInChildren<TextMeshProUGUI>().text  = "Save Game 1";
            }

            _gameSaveDataArray[1] = saveGame.LoadGameSaveData( 2 );

            if ( _gameSaveDataArray[1] != null )
            {
                _secondSlotButton.onClick.AddListener( () => {
                    _lastSelectedButton = _secondSlotButton;
                    OpenLoadConfirmationPanel( 1 );
                    } );

                _secondSlotButton.GetComponentInChildren<TextMeshProUGUI>().text = "Save Game 2";
            }

            _gameSaveDataArray[2] = saveGame.LoadGameSaveData( 3 );

            if ( _gameSaveDataArray[2] != null )
            {
                _thirdSlotButton.onClick.AddListener( () => {
                    _lastSelectedButton = _thirdSlotButton;
                    OpenLoadConfirmationPanel( 2 );
                    } );

                _thirdSlotButton.GetComponentInChildren<TextMeshProUGUI>().text  = "Save Game 3";
            }
        }

        private void OpenLoadConfirmationPanel( int saveSlot )
        {
            _slotToLoadIndex = saveSlot;

            _confirmationPanelUI.Show( LoadGame , () => {
                gameObject.SetActive( true );
                ServiceLocator.GetService<GameInputs>().OnCancelPerformed += GameInputs_OnCancelPerformed;
                _lastSelectedButton.Select();
            } ,
            _loadMessage + $"Save Game {saveSlot + 1}?" );

            gameObject.SetActive( false );
            ServiceLocator.GetService<GameInputs>().OnCancelPerformed -= GameInputs_OnCancelPerformed;
        }

        private void LoadGame()
        {
            _zoneExitSideSO.nextStartPointRefID  = _gameSaveDataArray[_slotToLoadIndex].startPointRefID;

            _playerStatusSaveSO.playerStatusSave = _gameSaveDataArray[_slotToLoadIndex].playerStatusSave;

            int arrayLength = _gameSaveDataArray[_slotToLoadIndex].zoneSavesArray.Length;
            for ( int i = 0; i < arrayLength; i++ )
                _gameZoneSavesSO.zones[i].zoneSave = _gameSaveDataArray[_slotToLoadIndex].zoneSavesArray[i];

            ServiceLocator.GetService<IAudioSpeaker>().ChangeMusic( MusicName.Woods_Zone_0 );
            switch ( _gameSaveDataArray[_slotToLoadIndex].startSavePoint )
            {
                case 0: ServiceLocator.GetService<SceneLoader>().Load( SceneName.S10_Z0_WoodsScene.ToString() );
                    break;
                case 1: ServiceLocator.GetService<SceneLoader>().Load( SceneName.S10_Z0_WoodsScene.ToString() );
                    break;
                case 2: ServiceLocator.GetService<SceneLoader>().Load( SceneName.S10_Z0_WoodsScene.ToString() );
                    break;
                case 3: ServiceLocator.GetService<SceneLoader>().Load( SceneName.S10_Z0_WoodsScene.ToString() );
                    break;
                default: Debug.LogError( $"Bad Load]: Start Save Point {_gameSaveDataArray[_slotToLoadIndex].startSavePoint} doesn't exist" );
                    break; 
            }
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