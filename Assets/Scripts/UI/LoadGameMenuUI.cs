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

        [Header("Save Slots Panel")]
        [SerializeField] private GameObject _confirmationPanel;

        private GameSaveData[] _gameSaveDataArray = new GameSaveData[3];


        private event Action OnReturnButtonClicked;


        private void Awake()
        {
            Instance = this;
            SetButtonEvents();
            SetSaveSlotInfo();
            Hide();
        }        

        private void SetButtonEvents()
        {
            _firstSlotButton.onClick.AddListener(  () => OpenSaveSlotPanel( 0 ) );
            _secondSlotButton.onClick.AddListener( () => OpenSaveSlotPanel( 1 ) );
            _thirdSlotButton.onClick.AddListener(  () => OpenSaveSlotPanel( 2 ) );

            _returnButton.onClick.AddListener( () => Hide() );
        }

        private void SetSaveSlotInfo()
        {
            SaveGame saveGame = new SaveGame();

            _gameSaveDataArray[0] = saveGame.LoadGameSaveData( 1 );
            if ( _gameSaveDataArray[0] != null )
                _firstSlotButton.GetComponentInChildren<TextMeshProUGUI>().text  = "Save Game 1";

            _gameSaveDataArray[1] = saveGame.LoadGameSaveData( 2 );
            if ( _gameSaveDataArray[1] != null )
                _secondSlotButton.GetComponentInChildren<TextMeshProUGUI>().text = "Save Game 2";

            _gameSaveDataArray[2] = saveGame.LoadGameSaveData( 3 );
            if ( _gameSaveDataArray[2] != null )
                _thirdSlotButton.GetComponentInChildren<TextMeshProUGUI>().text  = "Save Game 3";
        }

        private void OpenSaveSlotPanel( int saveSlot )
        {
            _confirmationPanel.SetActive( true );
            //_gameSaveDataArray[saveSlot];
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