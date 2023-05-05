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
    [Tooltip("Distancia a la que para si encuentra una pared.")]
    private float _wallAware = 0.5f;
    
    [SerializeField]
    [Tooltip("Tiempo en segundos para cambiar su dirección.")]
    private float _secondsToChangeDirection;

    [SerializeField]
    [Tooltip("Radio del sentido del oído.")]
    private float _earRadious;
    
    #endregion
    
    #region REFERENCES
    private ContactFilter2D _contactFilter = new ContactFilter2D();
    
    private LayerMask _layer = 0;

    private GameObject _player;

    private float _timer;
    
    private bool _canPatrol;

    private bool _canFollow;

    private bool _canCheckDistance;

    private bool _canSeePlayer;
    
    private NavMeshAgent _navMeshAgent;
    
    private float _nextWanderTime;

    private Collider2D boundsCollider;

    private FsmEnemySlime _actualState;
    public FsmEnemySlime ActualState
    {
        get => _actualState;
        set => _actualState = value;
    }

    #endregion
    
    #region UNITY METHODS
    private void Awake()
    {
        PrepareComponent();
    }
    
    void Start()
    {
        Init();
    }
    
    void Update()
    {
        _actualState.Execute(this);
    }
    
    private void PrepareComponent()
    {
        //Player
        if (_player == null)
            _player = FindGameObject.WithCaseInsensitiveTag(Constants.TAG_PLAYER);

        //NavMesh
        _navMeshAgent = GetComponent<NavMeshAgent>();
        
        //Collider donde patrulla el slime
        boundsCollider = FindGameObject.WithCaseInsensitiveTag(Constants.TAG_PATROL_COLLIDER).GetComponent<Collider2D>();
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
        
        Collider2D[] results = new Collider2D[5];

        int objectsDetected = Physics2D.OverlapCircle(transform.position, _earRadious, _contactFilter, results);

        if (objectsDetected > 0)
        {
            foreach (var item in results)
            {
                if (item != null)
                {
                    if (item.CompareTag(Constants.TAG_PLAYER))
                    {
                        _canSeePlayer = true;
                        break;
                    }
                }

            }
        }

        return _canSeePlayer;
    }

    public bool ObstacleAware()
    {
        bool result = false;
        
        Vector3 direction = transform.TransformDirection(_player.transform.position - transform.position);

        if (Physics2D.Raycast(transform.position, direction, _wallAware, _layer))
            result = true;
        
        return result;
 
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

    #endregion
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _earRadious);

        if (_player != null)
        {
            Gizmos.color = Color.blue;
            Vector3 direction = transform.TransformDirection(_player.transform.position - transform.position);
            Gizmos.DrawRay(transform.position, direction);
        
            Gizmos.color = Color.green;
            Vector3 direction2 = transform.TransformDirection(_player.transform.position - transform.position);
            Gizmos.DrawRay(transform.position, direction2 * 1.5f);
        }
   

    }
}