// ************ @autor: Álvaro Repiso Romero *************
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Xml.Linq;

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
        [SerializeField] private ZoneExitSideSO _zoneExitSideSO;
        [SerializeField] private ZoneSaveSO[] _zoneSaveSOList;

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
                NewGame();
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


        private void NewGame()
        {
            _zoneExitSideSO.NewGameReset();
            foreach ( var zoneSaveSO in _zoneSaveSOList )
                zoneSaveSO.NewGameReset();

            new SaveGame().SaveOptions( ServiceLocator.GetService<OptionsSave>() );

            ServiceLocator.GetService<SceneLoader>().Load( SceneName.S10_WOODS_Z0 );
            //ServiceLocator.GetService<SceneLoader>().Load( "ARR_Scene" );
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