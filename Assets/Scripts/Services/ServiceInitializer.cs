// ************ @autor: Álvaro Repiso Romero *************
using UnityEngine;
using static ServiceLocator;

namespace Services
{
    public class ServiceInitializer : MonoBehaviour
    {
        private void OnEnable()
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