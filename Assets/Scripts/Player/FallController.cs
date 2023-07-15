using UnityEngine;
using Player;

public class FallController
{
    private Rigidbody2D _rb;
    private Collider2D _collider;
    private AnimatorBrain _animatorBrain;
    private Jump _jump;

    private Vector2 _direction;
    private float _speed = 5f;
    private float _detectRadius = 0.05f;

    private Vector2 _resetPos;
    private Vector2 _resetLookDir;

    public bool IsFalling { get; private set; }
    public bool IsNotOnScreen { get; private set; }
    public bool HasFalled => IsFalling || IsNotOnScreen;

    public FallController( Rigidbody2D rb , Collider2D collider , AnimatorBrain animatorBrain , Jump jump )
    {
        _rb = rb;
        _collider = collider;
        _animatorBrain = animatorBrain;
        _jump = jump;
    }

    public void Init( Vector2 startPos , Vector2 startLookDirection )
    {
        _resetPos = startPos;
        _resetLookDir = startLookDirection;
    }

    public void SetFalling()
    {
        IsFalling = true;
        _animatorBrain.SetFall();
        _collider.enabled = false;
    }

    public void StartRecovering()
    {
        IsNotOnScreen = true;
        _direction = _resetPos - _rb.position;
        _direction.Normalize();
    }

    public void Move()
    {
        if ( HasReachDestination() )
        {
            Recovered();
        }
        else
        {
            _rb.MovePosition( _rb.position + Time.deltaTime * _speed * _direction );
            _rb.velocity = Vector2.zero;
        }
    }

    private bool HasReachDestination()
    {
        return ( _resetPos.x - _rb.position.x ) * ( _resetPos.x - _rb.position.x ) +
               ( _resetPos.y - _rb.position.y ) * ( _resetPos.y - _rb.position.y ) <
               _detectRadius * _detectRadius;
    }    

    private void Recovered()
    {
        IsNotOnScreen     = false;
        IsFalling         = false;
        _collider.enabled = true;
        _animatorBrain.RecoverFromFall( _resetLookDir );
    }
}
