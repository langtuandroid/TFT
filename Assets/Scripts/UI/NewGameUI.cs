using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class NewGameUI : MonoBehaviour
    {
        [Header("New Game Buttons")]
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
        private int _slotToSaveIndex;
        private string _overwriteMessage = "This slot is already in use, \n Do you want to overwrite it?";


        private event Action OnReturnButtonClicked;


        private void Start()
        {
            SetNewGamePossibleSlots();
            _returnButton.onClick.AddListener( () => Hide() );
            Hide();
        }

        private void SetNewGamePossibleSlots()
        {
            SaveGame saveGame = new SaveGame();

            _gameSaveDataArray[0] = saveGame.LoadGameSaveData( 1 );

            if ( _gameSaveDataArray[0] != null )
            {
                _firstSlotButton.onClick.AddListener( () => {
                    _lastSelectedButton = _firstSlotButton;
                    OpenOverwriteConfirmationPanel( 0 );
                } );

                _firstSlotButton.GetComponentInChildren<TextMeshProUGUI>().text = "Save Game 1";
            }
            else
            {
                _firstSlotButton.onClick.AddListener( () => {
                    _slotToSaveIndex = 0;
                    NewGame();
                } );
            }

            _gameSaveDataArray[1] = saveGame.LoadGameSaveData( 2 );

            if ( _gameSaveDataArray[1] != null )
            {
                _secondSlotButton.onClick.AddListener( () => {
                    _lastSelectedButton = _secondSlotButton;
                    OpenOverwriteConfirmationPanel( 1 );
                } );

                _secondSlotButton.GetComponentInChildren<TextMeshProUGUI>().text = "Save Game 2";
            }
            else
            {
                _secondSlotButton.onClick.AddListener( () => {
                    _slotToSaveIndex = 1;
                    NewGame();
                } );
            }

            _gameSaveDataArray[2] = saveGame.LoadGameSaveData( 3 );

            if ( _gameSaveDataArray[2] != null )
            {
                _thirdSlotButton.onClick.AddListener( () => {
                    _lastSelectedButton = _thirdSlotButton;
                    OpenOverwriteConfirmationPanel( 2 );
                } );

                _thirdSlotButton.GetComponentInChildren<TextMeshProUGUI>().text = "Save Game 3";
            }
            else
            {
                _thirdSlotButton.onClick.AddListener( () => {
                    _slotToSaveIndex = 2;
                    NewGame();
                } );
            }
        }

        private void OpenOverwriteConfirmationPanel( int saveSlot )
        {
            _slotToSaveIndex = saveSlot;

            _confirmationPanelUI.Show( NewGame , () => {
                gameObject.SetActive( true );
                ServiceLocator.GetService<GameInputs>().OnCancelPerformed += GameInputs_OnCancelPerformed;
                _lastSelectedButton.Select();
            } ,
            _overwriteMessage );

            gameObject.SetActive( false );
            ServiceLocator.GetService<GameInputs>().OnCancelPerformed -= GameInputs_OnCancelPerformed;
        }

        private void NewGame()
        {
            _playerStatusSaveSO.NewGameReset( _slotToSaveIndex + 1 );
            _zoneExitSideSO.NewGameReset();
            foreach ( var zoneSaveSO in _gameZoneSavesSO.zones )
                zoneSaveSO.NewGameReset();

            ServiceLocator.GetService<SceneLoader>().Load( SceneName.S10_WOODS_Z0 );
            //ServiceLocator.GetService<SceneLoader>().Load( "ARR_Scene" );
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