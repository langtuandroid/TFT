using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Utils;
using Random = UnityEngine.Random;

public class EnemySlime : MonoBehaviour
{
    #region CONFIGURATION
    [Header("Tags Necesarios:\n" +
            "Player: Transform del Player.\n" +
            "WayPoint: Transform de cada punto de platrulla.\n" +
            "PlayerInitialPosition: Transform de destino del Player\n cuando te alcanza.\n\n")]
    
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
    
    private List<GameObject> _torch;
    
    private List<Torch> _torchScript;
    
    private LayerMask _layer = 0;
    
    private ContactFilter2D _contactFilter = new ContactFilter2D();
    
    private Rigidbody2D _playerRB;
    
    private GameObject _player;
    
    private bool _facingRight;

    private float _timer;
    
    private bool _canPatrol;

    private bool _canFollow;

    private bool _canCheckDistance;

    private bool _canSeePlayer;
    
    private NavMeshAgent _navMeshAgent;
    
    private float _nextWanderTime;
    
    private float _wanderRadius = 5f;

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
        
        _playerRB = _player.GetComponent<Rigidbody2D>();

        //NavMesh
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Init()
    {
        if (transform.localScale.x < 0f) _facingRight = false;
        else if (transform.localScale.x > 0f) _facingRight = true;

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
                    if (item.CompareTag("Player"))
                    {
                        _canSeePlayer = true;
                        break;
                    }
                }

            }
        }

        return _canSeePlayer;
    }
    
    bool CanChangeDirection()
    {
        _timer += Time.deltaTime;

        if (_timer >= _secondsToChangeDirection) return true;
        else return false;
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
        Vector2 randomDirection = Random.insideUnitCircle * _wanderRadius;
        Vector2 targetPosition = (Vector2)transform.position + randomDirection;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPosition, out hit, _wanderRadius, NavMesh.AllAreas))
        {
            UpdatePatrolMovement(hit.position);
        }
    }

    public bool CanChangePatrolDirection()
    {
        var result = false;
        
        if (Time.time >= _secondsToChangeDirection)
        {
            result = true;
            _secondsToChangeDirection = Time.time + _secondsToChangeDirection;
        }

        return result;
    }
    
    
    
    public void Flip()
    {
        _timer = 0f;
        _facingRight = !_facingRight;
        float localScaleX = transform.localScale.x;
        localScaleX = localScaleX * -1f;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    public void UpdatePatrolMovement(Vector3 waypoint)
    {
        _navMeshAgent.destination = waypoint;
    }
    
    public void Follow()
    {
        _navMeshAgent.destination = _player.transform.position;
    }
    
    public void PatrolDirection()
    {
        Vector2 direction = Vector2.right;

        if (CanChangeDirection())
            Flip();

        if (!_facingRight)
            direction = Vector2.left;

        if (Physics2D.Raycast(transform.position, direction, _wallAware, _layer))
            Flip();
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
