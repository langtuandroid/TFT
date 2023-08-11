using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    public SkeletonDataSO SkeletonDataSO;

    public SkeletonBaseState CurrentState;

    private Rigidbody2D _rb;

    private float _waitSeconds;
    private float _maxWaitSeconds;
    private Vector2 _moveDir;

    private Transform _playerTrans;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        var skeletonStateFactory = new SkeletonStateFactory( this );
        CurrentState = skeletonStateFactory.Idle();
    }

    private void Start()  { CurrentState.EnterState(); }
    private void Update() { CurrentState.UpdateState(); }

    public bool IsMoving()
    {
        _waitSeconds += Time.deltaTime;
        return _waitSeconds > _maxWaitSeconds;
    }

    public void Movement()
    {
        bool canMove = Physics2D.Raycast( transform.position , _moveDir , 1 , SkeletonDataSO.ObstaclesMask );
        if ( canMove )
            _rb.MovePosition( _rb.position + Time.deltaTime * SkeletonDataSO.Speed * _moveDir );
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

    public void Attack()
    {
        Debug.Log( "Attack" );
        //GameObject bone = Instantiate( SkeletonDataSO.BonePrefab , transform.position , Quaternion.identity );
        //bone.GetComponent<Bone>().Throw( _playerTrans );
    }
}
