using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SceneLoadSystem;


namespace UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [Header("Main Menu Buttons")]
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _loadButton;
        [SerializeField] private Button _optionButton;
        [SerializeField] private Button _quitButton;


        private void Awake()
        {
            Show();
            SetButtonEvents();
        }

        private void SetButtonEvents()
        {
            _newGameButton.onClick.AddListener( () => {
                SceneLoader.Load( "SampleScene" );
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
            _newGameButton.Select();
        }

        private void Hide() => gameObject.SetActive( false );


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