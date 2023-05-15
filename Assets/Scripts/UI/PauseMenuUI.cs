using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        [Header("Main Menu Buttons")]
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _loadButton;
        [SerializeField] private Button _optionButton;
        [SerializeField] private Button _returnMainMenuButton;

        private Button _selectedButton;


        private void Awake()
        {
            _selectedButton = _resumeButton;
            SetButtonEvents();
            Hide();
        }

        private void SetButtonEvents()
        {
            _resumeButton.onClick.AddListener( () => {
                ServiceLocator.GetService<SceneLoader>().Load( "SampleScene" );
            } );

            _loadButton.onClick.AddListener( () => {
                Hide();
                LoadGameMenuUI.Instance.Show( Show );
            } );

            _optionButton.onClick.AddListener( () => {
                Hide();
                OptionMenuUI.Instance.Show( Show );
            } );

            _returnMainMenuButton.onClick.AddListener( () => QuitGame() );
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
            Debug.Log( "Return no Main Menu" );
        }
    }
}