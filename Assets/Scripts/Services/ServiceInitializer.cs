// ************ @autor: Álvaro Repiso Romero *************
using UnityEngine;

namespace Services
{
    public class ServiceInitializer : MonoBehaviour
    {
        private void OnEnable()
        {
            if ( !ServiceLocator.IsInitialized )
            {
                ServiceLocator.AddService( new GameInputs() );
                ServiceLocator.IsInitialized = true;
            }
            Destroy( gameObject );
        }
    }
}