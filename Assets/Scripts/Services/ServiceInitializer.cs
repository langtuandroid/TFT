// ************ @autor: Álvaro Repiso Romero *************
using UnityEngine;
using static ServiceLocator;

namespace Services
{
    public class ServiceInitializer : MonoBehaviour
    {
        private void Awake()
        {
            if ( !IsInitialized )
            {
                AddService( new GameInputs() );
                AddService(new MagicEvents());
                AddService( new AudioSpeaker() );

                IsInitialized = true;
            }
            Destroy( gameObject );
        }
    }
}