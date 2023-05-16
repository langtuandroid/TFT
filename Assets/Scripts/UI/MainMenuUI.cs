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
                ServiceLocator.GetService<SceneLoader>().Load( SceneName.S10_WOODS_Z0 );
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