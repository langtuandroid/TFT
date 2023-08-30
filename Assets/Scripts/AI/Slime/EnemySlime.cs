using System;
using System.Collections;
using Player;
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

    [SerializeField] 
    [Tooltip("Icono exclamación")]
    private GameObject _exclamation;
    
    [SerializeField]
    [Tooltip("Prefab Alma")]
    private GameObject _soulPrefab;
    
    #endregion
    
    #region REFERENCES
    private GameObject _player;

    private Collider2D boundsCollider;

    private float _timer;

    private bool _canCheckDistance;

    private bool _canSeePlayer;
    
    private NavMeshAgent _navMeshAgent;

    private SpriteRenderer _spriteRenderer;
    
    private float _nextWanderTime;

    private FsmEnemySlime _actualState;
    public FsmEnemySlime ActualState
    {
        get => _actualState;
        set => _actualState = value;
    }

    private bool _isOnProcedural;
    
    private bool _followState;
    
    private bool _canFollow;

    private bool _slimeLoaded;

    public bool SlimeLoaded { get => _slimeLoaded; }

    private LifeEvents _lifeEvents;

    private int numberOfSouls = 3;

    private SlimeHealth _slimeHealth;

    private bool _isAlive = true;

    #endregion
    
    #region UNITY METHODS

    private void OnDestroy()
    {
        if (_lifeEvents != null)
        {
            _lifeEvents.OnDeathValue -= OnStopFollow;
        }

        if (_slimeHealth != null)
        {
            _slimeHealth.OnDeath -= SlimeDead;
        }
    }


    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _slimeHealth = GetComponent<SlimeHealth>();
    }

    void Start()
    {
        _lifeEvents = ServiceLocator.GetService<LifeEvents>();
        _lifeEvents.OnDeathValue += OnStopFollow;
        _slimeHealth.OnDeath += SlimeDead;
        
        if ( !_isOnProcedural )
        {
            boundsCollider = GetComponentInParent<Collider2D>(); // si queremos que el slime este dentro de un area hay que añadir un collider al padre
            PrepareComponent();
            Init();
        }
    }
    
    void Update()
    {
        if (!_slimeLoaded) return;

        if ( _isOnProcedural )
        {
            Vector3 direction = _player.transform.position - transform.position;
            transform.position += Time.deltaTime * _speed * direction.normalized;
        }
        else
            _actualState.Execute(this);
        
        _spriteRenderer.flipX = _navMeshAgent.velocity.x < 0f;
    }

    private void SlimeDead()
    {
        _slimeLoaded = false;
        
        if (_navMeshAgent != null)
        {
            _navMeshAgent.speed = 0f;
            _navMeshAgent.isStopped = true;
        }
        
        for (int i = 0; i < numberOfSouls; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * 0.5f;
            Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);
            Instantiate(_soulPrefab, spawnPosition, Quaternion.identity);
        }
    }
    
    private void PrepareComponent()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _slimeLoaded = true;
    }

    private void Init()
    {
        _navMeshAgent.speed = _speed;

        _nextWanderTime = _secondsToChangeDirection;

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
                if (results.GetComponent<PlayerStatus>().HasTemporalInvencibility)
                    _canSeePlayer = false;
                else
                {
                    _canSeePlayer = true;
                    _player = results.gameObject;
                }
            }
        }

        if (_canSeePlayer && !_followState)
        {
            _followState = true;
            StartCoroutine(nameof(AlertExclamation));
        }
        
        return _canSeePlayer;
    }
    #endregion
    
    #region MOVEMENT

    public void Patrol()
    {
        if (TimeToChangeDirection())
        {
            _followState = false;
            _canFollow = false;
            UpdatePatrolMovement(GetRandomPosition());
        }
    }

    private void OnStopFollow()
    {
        _slimeLoaded = false;
    }
    
    private Vector3 GetRandomPosition()
    {
        //Vector2 randomPoint = Random.insideUnitCircle * boundsCollider.bounds.extents.x;
        Vector2 randomPoint = Random.insideUnitCircle * boundsCollider.bounds.extents.x * 5;
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
        if(_canFollow)
            UpdatePatrolMovement(_player.transform.position);
    }

    private IEnumerator AlertExclamation()
    {
        _exclamation.SetActive(true);
        
        yield return new WaitForSeconds(1f);
        
        _exclamation.SetActive(false);

        _canFollow = true;
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
