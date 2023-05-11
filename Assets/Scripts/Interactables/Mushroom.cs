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
        ChangeToJumpable( false );
    }

    public void ChangeToJumpable( bool isJumpable )
    {
        if ( isJumpable != _collider.isTrigger )
        {
            _collider.isTrigger = isJumpable;
            string result = isJumpable ? "ON" : "OFF";
            Debug.Log( "Jump in Mushroom: " + result );
        }
    }
}
