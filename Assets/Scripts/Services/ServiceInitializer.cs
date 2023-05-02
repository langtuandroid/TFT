// ************ @autor: �lvaro Repiso Romero *************
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

                IsInitialized = true;
            }
            Destroy( gameObject );
        }
    }
}