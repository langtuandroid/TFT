using System;
using UnityEngine;
using UnityEngine.AI;
using Utils;
using Random = UnityEngine.Random;

public class EnemySlime : MonoBehaviour
{
    #region CONFIGURATION
    [Header("Tags Necesarios:\n" +
            "Player: Transform del Player.\n" +
            "PatrolCollider: Área de patrulla.\n\n" +
            "1.- Añade un objeto vacío a la escena.\n" +
            "2.- Asigna el tag: 'PatrolCollider' a este objeto.\n" +
            "3.- Añade un BoxCollider2D.\n" +
            "4.- Marca el checkbox 'isTrigger'.\n\n\n")]
    
    [SerializeField]
    [Tooltip("Velocidad a la que se mueve el slime.")]
    private float _speed;

    [SerializeField]
    [Tooltip("Tiempo en segundos para cambiar su dirección.")]
    private float _secondsToChangeDirection;

    [SerializeField]
    [Tooltip("Radio del sentido del oído.")]
    private float _earRadious;

    #endregion
    
    #region REFERENCES
    private ContactFilter2D _contactFilter = new ContactFilter2D();
    
    private LayerMask _layer = 3;
    
    private GameObject _player;

    private Collider2D boundsCollider;

    private float _timer;
    
    private bool _canPatrol;

    private bool _canFollow;

    private bool _canCheckDistance;

    private bool _canSeePlayer;
    
    private NavMeshAgent _navMeshAgent;
    
    private float _nextWanderTime;

    private FsmEnemySlime _actualState;
    public FsmEnemySlime ActualState
    {
        get => _actualState;
        set => _actualState = value;
    }

    private bool _isOnProcedural;

    #endregion
    
    #region UNITY METHODS

    void Start()
    {
        if ( !_isOnProcedural )
        {
            boundsCollider = GetComponentInParent<Collider2D>();
            PrepareComponent();
            Init();
        }
    }
    
    void Update()
    {
        if ( _isOnProcedural )
        {
            
            Vector3 direction = _player.transform.position - transform.position;
            transform.position += Time.deltaTime * _speed * direction.normalized;
        }
        else
            _actualState.Execute(this);
    }
    
    private void PrepareComponent()
    {
        //Player
        //if (_player == null)
            //_player = FindGameObject.WithCaseInsensitiveTag(Constants.TAG_PLAYER);

        //NavMesh
        _navMeshAgent = GetComponent<NavMeshAgent>();
        
        //Collider donde patrulla el slime
        //boundsCollider = FindGameObject.WithCaseInsensitiveTag(Constants.TAG_PATROL_COLLIDER).GetComponent<Collider2D>();
        //boundsCollider = GetComponentInParent<Collider2D>();
    }

    private void Init()
    { 
        _navMeshAgent.speed = _speed;

        _actualState =  new EnemySlimePatrolState();
        
        Patrol();
    }
    
    public void ChangeState(FsmEnemySlime newState)
    {
        _actualState = newState;
    }
    
    #endregion
    
    #region SENSES
    
    public bool CanSeePlayer()
    {
        _canSeePlayer = false;

        //var hitColliders = Physics2D.OverlapCircle(center, radius, 1 << LayerMask.NameToLayer("Enemies")); 
        Collider2D results = Physics2D.OverlapCircle(transform.position, _earRadious, LayerMask.GetMask(Constants.TAG_PLAYER));

        if (results != null)
        {
            if (results.CompareTag(Constants.TAG_PLAYER))
            {
                _canSeePlayer = true;
                _player = results.gameObject;
            }
        }
        
        return _canSeePlayer;
    }
    #endregion
    
    #region MOVEMENT

    public void Patrol()
    {
        if (TimeToChangeDirection())
        {
            UpdatePatrolMovement(GetRandomPosition());
        }
    }

    public void ChangeDirectionAndPatrol()
    {
        UpdatePatrolMovement(GetRandomPosition());
    }
    
    private Vector3 GetRandomPosition()
    {
        Vector2 randomPoint = Random.insideUnitCircle * boundsCollider.bounds.extents.x;
        Vector3 targetPosition = new Vector3(boundsCollider.bounds.center.x + randomPoint.x, boundsCollider.bounds.center.y + randomPoint.y, 0);

        return targetPosition;
    }

    public bool TimeToChangeDirection()
    {
        var result = false;

        if (Time.time >= _nextWanderTime)
        {
            result = true;
            _nextWanderTime = Time.time + _secondsToChangeDirection;
        }

        return result;
    }
    
    public void UpdatePatrolMovement(Vector3 waypoint)
    {
        _navMeshAgent.destination = waypoint;
    }
    
    public void Follow()
    {
        _navMeshAgent.destination = _player.transform.position;
    }

    public void SetAsProceduralEnemy( Transform playerTransform )
    {
        _isOnProcedural = true;
        _player = playerTransform.gameObject;
        GetComponent<NavMeshAgent>().enabled = false;
    }

    #endregion

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _earRadious);
    }
}
