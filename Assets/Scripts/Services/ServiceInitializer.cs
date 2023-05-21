// ************ @autor: Álvaro Repiso Romero *************
using UnityEngine;
using static ServiceLocator;

namespace Services
{
    [DefaultExecutionOrder(-5)]
    public class ServiceInitializer : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = Screen.currentResolution.refreshRate;
            if (!IsInitialized)
            {
                // Load or Initialize OptionsSave
                SaveGame saveGame = new SaveGame();
                OptionsSave optionsSave = saveGame.LoadOptions();
                // Systems
                AddService( optionsSave );
                AddService( new GameInputs( optionsSave ) );
                AddService( new AudioSpeaker() );
                AddService( new SceneLoader() );
                // Events
                AddService( new MagicEvents() );
                AddService( new LevelEvents() );

                IsInitialized = true;
            }
            Destroy(gameObject);
        }
    }
}