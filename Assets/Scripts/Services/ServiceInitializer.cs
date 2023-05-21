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
            if (!IsInitialized)
            {
                // Load or Initialize OptionsSave
                OptionsSave optionsSave = new SaveGame().Get().LoadOptions();
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