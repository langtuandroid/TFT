// ************ @autor: Álvaro Repiso Romero *************
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [Header("Main Menu Buttons")]
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _loadButton;
        [SerializeField] private Button _optionButton;
        [SerializeField] private Button _quitButton;

        [Header("Save Data")]
        [SerializeField] private PlayerStatusSaveSO _playerStatusSaveSO;
        [SerializeField] private ZoneExitSideSO     _zoneExitSideSO;
        [SerializeField] private GameZoneSavesSO    _gameZoneSavesSO;

        private Button _selectedButton;


        private void Awake()
        {
            _selectedButton = _newGameButton;
            Show();
            SetButtonEvents();
        }

        private void SetButtonEvents()
        {
            _newGameButton.onClick.AddListener( () => {
                Hide();
                NewGameUI.Instance.Show( Show );
                //NewGame();
            } );

            _loadButton.onClick.AddListener( () => {
                Hide();
                LoadGameMenuUI.Instance.Show( Show );
            } );

            _optionButton.onClick.AddListener( () => {
                Hide();
                OptionMenuUI.Instance.Show( Show );
            } );

            _quitButton.onClick.AddListener( () => QuitGame() );
        }

        private void Show()
        {
            gameObject.SetActive( true );
            _selectedButton.Select();
        }

        private void Hide() 
        { 
            _selectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>(); 
            gameObject.SetActive( false ); 
        }


        private void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}