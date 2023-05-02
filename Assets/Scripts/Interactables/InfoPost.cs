using UnityEngine;

public class InfoPost : MonoBehaviour, IInteractable
{
    [SerializeField] private string InfoPostMessage;

    [SerializeField] private bool canRead = false;

    public void Interact( Vector2 lookDirection )
    {
        Debug.Log( lookDirection );
        if ( canRead && lookDirection.y > 1 )
            Debug.Log( InfoPostMessage );
        else
            Debug.Log( "Cannot Read From Here" );
    }

    private void OnCollisionEnter2D( Collision2D collision )
    {
        if ( collision.transform.CompareTag( "Player" ) )
            canRead = true;
    }

    private void OnCollisionExit2D( Collision2D collision )
    {
        if ( collision.transform.CompareTag( "Player" ) )
            canRead = false;
    }

    private void OnTriggerEnter2D( Collider2D collision )
    {
        if ( collision.CompareTag( "Player" ) )
            canRead = true;
    }

    private void OnTriggerExit2D( Collider2D collision )
    {
        if ( collision.CompareTag( "Player" ) )
            canRead = false;
    }
}
