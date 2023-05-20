// ************ @autor: Álvaro Repiso Romero *************
using UnityEngine;
using static ServiceLocator;

namespace Services
{
    public class ServiceInitializer : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = Screen.currentResolution.refreshRate;
            if (!IsInitialized)
            {
                // Systems
                AddService( new GameInputs() );
                AddService( new AudioSpeaker() );
                AddService( new SceneLoader() );
                AddService( new OptionsSave() );
                // Events
                AddService( new MagicEvents() );
                AddService( new LevelEvents() );

                IsInitialized = true;
            }
            Destroy(gameObject);
        }
    }
}