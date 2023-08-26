using UnityEngine;
using Player;

public class FallController
{
    private Rigidbody2D _rb;
    private Collider2D  _collider;

    private AnimatorBrain _animatorBrain;
    private IAudioSpeaker _audioSpeaker;
    private GameStatus    _gameStatus;
    private PlayerStatus  _playerStatus;

    private Vector2 _resetPos;
    private Vector2 _direction;
    private float _speed;
    private float _detectionRadius;

    public bool HasFalled { get; private set; }
    public bool IsNotOnScreen { get; private set; }

    public FallController( Rigidbody2D rb , Collider2D collider , AnimatorBrain animatorBrain , 
        PlayerPhysicalDataSO playerPhysicalDataSO )
    {
        _rb              = rb;
        _collider        = collider;
        _animatorBrain   = animatorBrain;
        _speed           = playerPhysicalDataSO.startPointRecoverSpeed;
        _detectionRadius = playerPhysicalDataSO.detectionRadius;
    }

    public void Init( Vector2 startPos , IAudioSpeaker audioSpeaker , GameStatus gameStatus , PlayerStatus playerStatus )
    {
        _resetPos     = startPos;
        _audioSpeaker = audioSpeaker;
        _gameStatus   = gameStatus;
        _playerStatus = playerStatus;
    }

    public void AddGravity( Vector2 direction )
    {
        if ( !Physics2D.CircleCast( _rb.position + _collider.offset , 0.5f , Vector2.zero , 0.01f , 1 << 10 ) )
        {
            float _gravityForce = 2.5f;
            _rb.AddForce( direction.normalized * _gravityForce , ForceMode2D.Force );
        }
    }

    public bool CanFall()
    {
        return !Physics2D.OverlapPoint( _rb.position + _collider.offset , 1 << 10 );
    }

    public void SetFalling( Vector2 centerPosition )
    {
        //_audioSpeaker.PlaySound( AudioID.G_PLAYER , AudioID.S_FALL );
        _rb.position = centerPosition;
        HasFalled = true;
        _animatorBrain.SetFall();
        _collider.enabled = false;
        _gameStatus.AskChangeToInactiveState();
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
            HasFalled         = false;
            _collider.enabled = true;
            _animatorBrain.RecoverFromFall();
            _gameStatus.AskChangeToGamePlayState();
            _playerStatus.TakeDamage( 1 );
        }
        else
        {
            _rb.MovePosition( _rb.position + Time.deltaTime * _speed * _direction );
        }
    }

    private bool HasReachDestination()
    {
        return ( _resetPos.x - _rb.position.x ) * ( _resetPos.x - _rb.position.x ) +
               ( _resetPos.y - _rb.position.y ) * ( _resetPos.y - _rb.position.y ) <
               _detectionRadius * _detectionRadius;
    }
}
