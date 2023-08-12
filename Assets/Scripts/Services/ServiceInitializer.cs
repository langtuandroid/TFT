// ************ @autor: Álvaro Repiso Romero *************

using Honeti;
using UnityEngine;
using static ServiceLocator;

namespace Services
{
    [DefaultExecutionOrder(-5)]
    public class ServiceInitializer : MonoBehaviour
    {
#if UNITY_EDITOR
        public bool IsRealMusicPlaying;
#endif

        private void Awake()
        {
            if (!IsInitialized)
            {
                // Load or Initialize OptionsSave
                OptionsSave optionsSave = new SaveGame().LoadOptions();
                GameStatus gameStatus = new GameStatus();

                // Systems
                AddService( gameStatus );
                AddService(optionsSave);
                AddService(new GameInputs(optionsSave, gameStatus));
                AddService(new SceneLoader());

//#if UNITY_EDITOR
//                IAudioSpeaker audio = IsRealMusicPlaying ? AudioManager.Instance : new DummyAudio();
//#else
//                IAudioSpeaker audio = AudioManager.Instance;
//#endif
//                AddService( audio );

                // Events
                AddService(new MagicEvents());
                AddService(new InventoryEvents());
                AddService(new LevelEvents());
                AddService(new LifeEvents());
                AddService(new SoulEvents());

                IsInitialized = true;
            }
            Destroy(gameObject);
        }
    }
}