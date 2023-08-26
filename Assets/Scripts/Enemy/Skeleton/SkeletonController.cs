using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    public SkeletonDataSO SkeletonDataSO;

    public SkeletonBaseState CurrentState;

    private Rigidbody2D _rb;
    private Animator _anim;
    private SpriteRenderer _spriteRenderer;

    private float _maxActionSeconds;
    private float _actionSeconds;
    private float _attackCounterSeconds;
    private Vector2 _moveDir;
    private Vector2[] _directionArray = { Vector2.up, Vector2.down, Vector2.left, Vector2.right, Vector2.zero };

    private Transform _playerTrans;
    private ObjectPool _bonePool;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        //_bonePool = GetComponentInChildren<ObjectPool>();
        _bonePool = new ObjectPool( SkeletonDataSO.BonePrefab , 3 );
        var skeletonStateFactory = new SkeletonStateFactory( this );
        CurrentState = skeletonStateFactory.Idle();
    }

    private void Start()  { CurrentState.EnterState(); }
    private void Update() { CurrentState.UpdateState(); }

    public bool CanChangeMoveDirection()
    {
        _actionSeconds += Time.deltaTime;
        return _actionSeconds > _maxActionSeconds;
    }

    public void ResetMove()
    {
        _maxActionSeconds = Random.Range( 1 , 3f );
        _actionSeconds = 0;
        _moveDir = SelectRandomDirection();
    }

    private Vector2 SelectRandomDirection()
    {
        return _directionArray[Random.Range( 0 , _directionArray.Length )];
    }

    public bool CanMove()
    {
        return Physics2D.Raycast( transform.position , _moveDir , 1 , SkeletonDataSO.ObstaclesMask );
    }

    public void Movement()
    {
        if ( CanMove() )
            _rb.MovePosition( _rb.position + Time.deltaTime * SkeletonDataSO.Speed * _moveDir );
        else
            _moveDir = _directionArray[Random.Range( 0 , _directionArray.Length )];

        MoveAnimation();
    }

    public void MoveAnimation()
    {
        if ( _moveDir.Equals( Vector2.zero ) )
        {
            _anim.Play( "Idle" );
        }
        else 
        if ( _moveDir.Equals( Vector2.left ) )
        {
            _anim.Play( "Move" );
            _spriteRenderer.flipX = true;
        }
        else
        if ( _moveDir.Equals( Vector2.right ) )
        {
            _anim.Play( "Move" );
            _spriteRenderer.flipX = false;
        }
    }

    public void ResetPursuit()
    {
        _maxActionSeconds = Random.Range( 1 , 3f );
        _actionSeconds = 0;
        _moveDir = NextPursuitDirection();

        MoveAnimation();
    }

    public void Pursuit()
    {
        if ( CanMove() )
            _rb.MovePosition( _rb.position + Time.deltaTime * SkeletonDataSO.Speed * _moveDir );
        else
            _moveDir = NextPursuitDirection();
    }

    public Vector2 NextPursuitDirection()
    {
        var dirSelector = Random.Range( 0 , 100 );

        // Bias pursuit player
        if ( dirSelector < 75 )
        {
            Vector2 optPursuitDirection = _playerTrans.position - transform.position;

            int x = 0;
            int y = 1;

            if ( Mathf.Abs( optPursuitDirection.x ) > Mathf.Abs( optPursuitDirection.y ) )
            {
                x = 1;
                y = 0;
            }

            if ( optPursuitDirection.x < 0 ) x *= -1;
            if ( optPursuitDirection.y < 0 ) y *= -1;

            return new Vector2( x, y );
        }

        return SelectRandomDirection();
    }

    public bool HasDetectPlayer()
    {
        var playerCol = Physics2D.OverlapCircle( transform.position , SkeletonDataSO.DetectionRadius , SkeletonDataSO.PlayerMask );
        if ( playerCol )
        {
            if ( _playerTrans == null ) _playerTrans = playerCol.transform;
            return true;
        }
        return false;
    }

    public bool IsReadyToAttack()
    {
        _attackCounterSeconds -= Time.deltaTime;
        return _attackCounterSeconds < 0;
    }

    public void Attack()
    {
        _attackCounterSeconds = SkeletonDataSO.AttackIntervalSeconds;
        var bone = _bonePool.GetPooledObject();
        bone.transform.position = transform.position;
        bone.GetComponent<BoneProjectile>().Launch( _playerTrans , SkeletonDataSO.Damage );
    }
}
