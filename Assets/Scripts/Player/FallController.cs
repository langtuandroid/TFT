using UnityEngine;
using Player;

public class FallController
{
    private Rigidbody2D _rb;
    private Collider2D _collider;
    private AnimatorBrain _animatorBrain;
    private IAudioSpeaker _audioSpeaker;

    private Vector2 _resetPos;
    private Vector2 _direction;
    private float _speed;
    private float _detectionRadius;

    public bool IsFalling { get; private set; }
    public bool IsNotOnScreen { get; private set; }
    public bool HasFalled => IsFalling || IsNotOnScreen;

    public FallController( Rigidbody2D rb , Collider2D collider , AnimatorBrain animatorBrain , 
        PlayerPhysicalDataSO playerPhysicalDataSO )
    {
        _rb = rb;
        _collider = collider;
        _animatorBrain = animatorBrain;
        _speed = playerPhysicalDataSO.startPointRecoverSpeed;
        _detectionRadius = playerPhysicalDataSO.detectionRadius;
    }

    public void Init( Vector2 startPos , IAudioSpeaker audioSpeaker )
    {
        _resetPos = startPos;
        _audioSpeaker = audioSpeaker;
    }

    public void SetFalling()
    {
        //_audioSpeaker.PlaySound();
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
            // Has recovered
            IsNotOnScreen     = false;
            IsFalling         = false;
            _collider.enabled = true;
            _animatorBrain.RecoverFromFall();
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
               _detectionRadius * _detectionRadius;
    }
}
