// ************ @autor: Álvaro Repiso Romero *************
using UnityEngine;
using static ServiceLocator;

namespace Services
{
    public class ServiceInitializer : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
            if (!IsInitialized)
            {
                // Systems
                AddService( new GameInputs() );
                AddService( new AudioSpeaker() );
                AddService( new SceneLoader() );
                // Events
                AddService( new MagicEvents() );

                IsInitialized = true;
            }
            Destroy(gameObject);
        }
    }
}