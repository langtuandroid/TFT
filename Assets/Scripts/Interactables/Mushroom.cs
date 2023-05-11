using UnityEngine;

public class Mushroom : MonoBehaviour, IJumpable
{
    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    public void JumpIn( Transform transform )
    {
        transform.position += Vector3.up * 2;
        _collider.isTrigger = false;
        Debug.Log( "Jump in Mushroom: OFF" );
    }

    public void ChangeToJumpable( bool isJumpable )
    {
        if ( isJumpable != _collider.isTrigger )
        {
            _collider.isTrigger = isJumpable;
            string message = "Jump in Mushroom: ";
            message += isJumpable ? "ON" : "OFF";
            Debug.Log( message );
        }
    }
}
