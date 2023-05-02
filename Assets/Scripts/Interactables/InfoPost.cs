using UnityEngine;

public class InfoPost : MonoBehaviour, IInteractable
{
    [SerializeField] private string InfoPostMessage;

    private bool canRead = false;

    public void Interact()
    {
        if ( canRead )
            Debug.Log( InfoPostMessage );
        else
            Debug.Log( "Cannot Read From Here" );
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
