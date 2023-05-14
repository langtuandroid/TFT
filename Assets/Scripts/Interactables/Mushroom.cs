using UnityEngine;

public class Mushroom : MonoBehaviour, IJumpable
{
    private Collider2D _collider;
    private Animator _animator;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
    }

    public void JumpIn()
    {
        _animator.SetTrigger( "Boing" );
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
