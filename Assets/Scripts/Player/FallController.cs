using UnityEngine;
using Player;

public class FallController
{
    private Rigidbody2D _rb;
    private Collider2D _collider;
    private AnimatorBrain _animatorBrain;

    private Vector2 _direction;
    private float _speed = 5f;
    private float _detectRadius = 0.05f;

    private Vector2 _resetPos;
    private Vector2 _resetLookDir;
    private LayerMask _resetLayerMask;

    public bool IsFalling { get; private set; }
    public bool IsNotOnScreen { get; private set; }
    public bool HasFalled => IsFalling || IsNotOnScreen;

    public FallController( Rigidbody2D rb , Collider2D collider , AnimatorBrain animatorBrain )
    {
        _rb = rb;
        _collider = collider;
        _animatorBrain = animatorBrain;
    }

    public void Init( Vector2 startPos , Vector2 startLookDirection , LayerMask initialGroundLayerMask )
    {
        _resetPos = startPos;
        _resetLookDir = startLookDirection;
        _resetLayerMask = initialGroundLayerMask;
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
