using UnityEngine;

public class InfoPost : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _exclamationIcon;
    [SerializeField] private string _infoPostMessage;

    public void Interact( Vector2 lookDirection )
    {
        if ( lookDirection.y > 0 )
            Debug.Log( _infoPostMessage );
        else
            Debug.Log( "Cannot Read From Here" );
    }

    public void ShowCanInteract( bool show )
    {
        _exclamationIcon.SetActive( show );
    }
}
