// ************ @autor: Álvaro Repiso Romero *************
using UnityEngine;
using static ServiceLocator;

namespace Services
{
    [DefaultExecutionOrder(-5)]
    public class ServiceInitializer : MonoBehaviour
    {
        public bool IsRealMusicPlaying;

        private void Awake()
        {            
            if (!IsInitialized)
            {
                // Load or Initialize OptionsSave
                OptionsSave optionsSave = new SaveGame().LoadOptions();
                // Systems
                AddService( optionsSave );
                AddService( new GameInputs( optionsSave ) );
#if UNITY_EDITOR
                AddService( new AudioSpeaker( IsRealMusicPlaying ) );
#else
                AddService( new AudioSpeaker( true ) );
#endif
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