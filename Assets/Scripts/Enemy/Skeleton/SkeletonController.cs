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
    private bool _isMoving;
    private Vector2[] _directionArray = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

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

    public bool CanChangeAction()
    {
        _actionSeconds += Time.deltaTime;
        return _actionSeconds > _maxActionSeconds;
    }

    public void ResetAction()
    {
        _maxActionSeconds = Random.Range( 1 , 3f );
        _actionSeconds = 0;
        _isMoving = !_isMoving;        
        _moveDir = _directionArray[Random.Range( 0 , _directionArray.Length )];
    }

    public void Movement()
    {
        if ( _isMoving )
        {
            bool canMove = Physics2D.Raycast( transform.position , _moveDir , 1 , SkeletonDataSO.ObstaclesMask );
            if ( canMove )
                _rb.MovePosition( _rb.position + Time.deltaTime * SkeletonDataSO.Speed * _moveDir );            
        }
    }

    public bool HasDetectPlayer()
    {
        Collider2D playerCol = Physics2D.OverlapCircle( transform.position , SkeletonDataSO.DetectionRadius , SkeletonDataSO.PlayerMask );
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
        GameObject bone = Instantiate( SkeletonDataSO.BonePrefab , transform.position , Quaternion.identity );
        bone.GetComponent<BoneProjectile>().Launch( _playerTrans , SkeletonDataSO.Damage );
    }
}
