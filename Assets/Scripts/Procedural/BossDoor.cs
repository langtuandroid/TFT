using UnityEngine;

namespace Procedural
{
    public class BossDoor : MonoBehaviour
    {
        private bool _hasKey;

        private void OnTriggerEnter2D( Collider2D collision )
        {
            if ( _hasKey )
                if ( collision.gameObject.tag == "Player" )
                {
                    Debug.Log( "door Open" );
                }
            else
                    Debug.Log( "No key" );
        }

        public void HasGetKey() => _hasKey = true;
    }
}