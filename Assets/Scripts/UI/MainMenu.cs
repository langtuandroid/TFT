using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _loadButton;
        [SerializeField] private Button _optionButton;

        [SerializeField] private GameObject _mainMenuPanel;
        [SerializeField] private GameObject _newGamePanel;
        [SerializeField] private GameObject _loadPanel;
        [SerializeField] private GameObject _optionPanel;


        private void Awake()
        {
            InitMenu();
            SetButtonEvents();
        }

        private void InitMenu()
        {
            _mainMenuPanel.SetActive( true );
            _newGamePanel.SetActive( false );
            _loadPanel.SetActive( false );
            _optionPanel.SetActive( false );
        }

        private void SetButtonEvents()
        {
            _newGameButton.onClick.AddListener( () => NewGame() );
            _loadButton.onClick.AddListener( () => LoadGame() );
            _optionButton.onClick.AddListener( () => OptionMenu() );
        }


        private void NewGame()
        {
            _mainMenuPanel.SetActive( false );
            _newGamePanel.SetActive( true );
        }

        private void LoadGame()
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