using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Main Menu Buttons")]
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _loadButton;
        [SerializeField] private Button _optionButton;
        
        [Header("Return To Main Menu Buttons")]
        [SerializeField] private Button _newGameReturnButton;
        [SerializeField] private Button _loadReturnButton;
        [SerializeField] private Button _optionReturnButton;

        [Header("Main Menu Scene Panels")]
        [SerializeField] private GameObject _mainMenuPanel;
        [SerializeField] private GameObject _newGamePanel;
        [SerializeField] private GameObject _loadPanel;
        [SerializeField] private GameObject _optionPanel;


        private void Awake()
        {
            ReturnMainMenu();
            SetButtonEvents();
        }

        private void SetButtonEvents()
        {
            _newGameButton.onClick.AddListener( () => NewGameMenu() );
            _loadButton.onClick.AddListener( () => LoadGameMenu() );
            _optionButton.onClick.AddListener( () => OptionMenu() );

            _newGameReturnButton.onClick.AddListener( () => ReturnMainMenu() );
            _loadReturnButton.onClick.AddListener( () => ReturnMainMenu() );
            _optionReturnButton.onClick.AddListener( () => ReturnMainMenu() );
        }


        private void ReturnMainMenu()
        {
            _mainMenuPanel.SetActive( true );
            _newGamePanel.SetActive( false );
            _loadPanel.SetActive( false );
            _optionPanel.SetActive( false );
        }


        private void NewGameMenu()
        {
            _mainMenuPanel.SetActive( false );
            _newGamePanel.SetActive( true );
        }


        private void LoadGameMenu()
        {
            _mainMenuPanel.SetActive( false );
            _loadPanel.SetActive( true );
        }


        private void OptionMenu()
        {
            _mainMenuPanel.SetActive( false );
            _optionPanel.SetActive( true );
        }
    }
}