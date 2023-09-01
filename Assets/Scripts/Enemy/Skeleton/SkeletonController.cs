using UnityEngine;

public class SkeletonController : MonoBehaviour, IBurnable, IDungeonInstantiable, IEnemyDeath
{
    [SerializeField] private SkeletonDataSO _skeletonDataSO;

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
    private int _currentHealth;
    private bool _isAlive = true;

    public event System.Action OnDeath;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _bonePool = new ObjectPool( _skeletonDataSO.BonePrefab , _skeletonDataSO.NumOfBones );
        var skeletonStateFactory = new SkeletonStateFactory( this );
        CurrentState = skeletonStateFactory.Idle();

        _currentHealth = _skeletonDataSO.Health;
    }

    private void Start()  { CurrentState.EnterState(); }
    private void Update() 
    {
        if ( _isAlive )
        {
            CurrentState.UpdateState();
        }
        else
        {
            _actionSeconds += Time.deltaTime;
            var deathAnimationSeconds = _anim.GetCurrentAnimatorStateInfo( 0 ).length + 1f;
            if ( _actionSeconds > deathAnimationSeconds )
            {
                Destroy( gameObject );
            }
        }
    }

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
        var rayDistance = 1;
        return Physics2D.Raycast( transform.position , _moveDir , rayDistance , _skeletonDataSO.ObstaclesMask );
    }

    public void Movement()
    {
        if ( CanMove() )
            _rb.MovePosition( _rb.position + Time.deltaTime * _skeletonDataSO.Speed * _moveDir );
        else
            _moveDir = SelectRandomDirection();

        MoveAnimation();
    }

    public void MoveAnimation()
    {
        if ( _moveDir.Equals( Vector2.zero ) )
        {
            _anim.Play( "Idle" );
        }
        else
        {
            _anim.Play( "Move" );
            if ( _moveDir.Equals( Vector2.left ) )  _spriteRenderer.flipX = true;
            else
            if ( _moveDir.Equals( Vector2.right ) ) _spriteRenderer.flipX = false;
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
            _rb.MovePosition( _rb.position + Time.deltaTime * _skeletonDataSO.Speed * _moveDir );
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
        var playerCol = Physics2D.OverlapCircle( transform.position , _skeletonDataSO.DetectionRadius , _skeletonDataSO.PlayerMask );
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
        _anim.Play( "Attack" );

        _spriteRenderer.flipX = transform.position.x - _playerTrans.position.x > 0;

        _attackCounterSeconds = _skeletonDataSO.AttackIntervalSeconds;
        _actionSeconds = 0;

        var bone = _bonePool.GetPooledObject();
        bone.transform.position = transform.position;
        bone.GetComponent<BoneProjectile>().Launch( _playerTrans , _skeletonDataSO.Damage );
    }

    public bool HasEndAttack()
    {
        _actionSeconds += Time.deltaTime;
        return _actionSeconds > _anim.GetCurrentAnimatorStateInfo( 0 ).length + 0.1f;
    }

    private void TakeDamage( int damage )
    {
        _currentHealth -= damage;
        if ( _currentHealth <= 0 )
        {
            _anim.Play( "Death" );
            _isAlive = false;
            _actionSeconds = 0;
            OnDeath?.Invoke();
        }
    }

    public void Burn( int damage )
    {
        TakeDamage( damage );
    }

    public void SetAsProceduralEnemy( Transform playerTransform ) { }

    public void OnEnemyDeath( System.Action functionToCall )
    {
        OnDeath += functionToCall;
    }
}
