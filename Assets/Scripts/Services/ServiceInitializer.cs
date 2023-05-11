// ************ @autor: Álvaro Repiso Romero *************
using UnityEngine;
using static ServiceLocator;

namespace Services
{
    public class ServiceInitializer : MonoBehaviour
    {
        private void Awake()
        {
            if (!IsInitialized)
            {
                // Systems
                AddService( new GameInputs() );
                AddService( new AudioSpeaker() );
                // Events
                AddService( new MagicEvents() );
                AddService( new JumpEvents() );


                IsInitialized = true;
            }
            Destroy(gameObject);
        }
    }
}