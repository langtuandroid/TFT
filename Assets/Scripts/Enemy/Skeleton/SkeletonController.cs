using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    public SkeletonDataSO SkeletonDataSO;

    public SkeletonBaseState CurrentState;

    private Rigidbody2D _rb;

    private float _maxActionSeconds;
    private float _actionSeconds;
    private float _attackIntervalSeconds = 2;
    private float _attackCounterSeconds;
    private Vector2 _moveDir;
    private Vector2[] _directionArray = { Vector2.up, Vector2.down, Vector2.left, Vector2.right, Vector2.zero };

    private Transform _playerTrans;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        var skeletonStateFactory = new SkeletonStateFactory( this );
        CurrentState = skeletonStateFactory.Idle();
        _moveDir = Vector2.left;
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
    }

    public void ResetPursuit()
    {
        _maxActionSeconds = Random.Range( 1 , 3f );
        _actionSeconds = 0;
        _moveDir = NextPursuitDirection();
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
        _attackCounterSeconds += Time.deltaTime;
        return _attackCounterSeconds > _attackIntervalSeconds;
    }

    public void Attack()
    {
        _attackCounterSeconds = 0;
        var bone = Instantiate( SkeletonDataSO.BonePrefab , transform.position , Quaternion.identity );
        bone.GetComponent<BoneProjectile>().Launch( _playerTrans , SkeletonDataSO.Damage );
    }
}
