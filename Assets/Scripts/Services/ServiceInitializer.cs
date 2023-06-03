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
                IAudioSpeaker audio = IsRealMusicPlaying ? AudioManager.Instance : new DummyAudio();
#else
                IAudioSpeaker audio = AudioManager.Instance;
#endif
                AddService( audio );
                AddService( new SceneLoader() );
                // Events
                AddService( new MagicEvents() );
                AddService( new LevelEvents() );
                AddService( new LifeEvents() );

                IsInitialized = true;
            }
            Destroy(gameObject);
        }
    }
}