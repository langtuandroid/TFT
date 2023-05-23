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

        [Header("Menus")]
        [SerializeField] private NewGameUI      _newGameUI;
        [SerializeField] private LoadGameMenuUI _loadGameMenuUI;
        [SerializeField] private OptionMenuUI   _optionMenuUI;


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
                _newGameUI.Show( Show );
            } );

            _loadButton.onClick.AddListener( () => {
                Hide();
                _loadGameMenuUI.Show( Show );
            } );

            _optionButton.onClick.AddListener( () => {
                Hide();
                _optionMenuUI.Show( Show );
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