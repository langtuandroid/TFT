using UnityEngine;

public class TriggerAudioChange : MonoBehaviour
{
    private void OnTriggerEnter2D( Collider2D collision )
    {
        if ( collision.CompareTag( "Player" ) )
        {
            AudioManager.Instance.ChangeMusic();
        }
    }
}
