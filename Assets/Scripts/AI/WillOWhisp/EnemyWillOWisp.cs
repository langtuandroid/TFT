using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Utils;
using DG.Tweening;
using UnityEngine.Rendering.Universal;
using System.Linq;

namespace AI
{
    public class EnemyWillOWisp : MonoBehaviour
    {        
    #region CONFIGURATION

    [Header("Tags Necesarios:\n" +
            "Player: Transform del Player.\n" +
            "WayPoint: Transform de cada punto de platrulla.\n" +
            "PlayerInitialPosition: Transform de destino del Player\n cuando te alcanza.\n\n")]
    
    [SerializeField]
    [Tooltip("Lista de waypoints para patrulla.")]
    private List<Transform> _wayPointsList;
    
    [SerializeField]
    [Tooltip("Lista de antorchas para apagar.")]
    private List<GameObject> _torchList;
    
    [SerializeField]
    [Tooltip("Posición original a la que mandamos al player.")]
    private Transform _playerInitialPosition;
    
    [SerializeField]
    [Tooltip("Posición original a la que mandamos al player.")]
    private Light2D _light2D;
    
    [SerializeField]
    [Tooltip("Radio del sentido escucha.")]
    private float _sightRadio;
    
    [SerializeField]
    [Tooltip("Distancia a la que vemos al player.")]
    private float _sightAware;
    
    [SerializeField]
    [Tooltip("Distancia mínima para ser alcanzado.")]
    private float _playerFollowDistance;
    
    [SerializeField] 
    [Tooltip("Tiempo que tarda en volver a patrulla desde cualquier sentido.")]
    private float _resetSeconds;
    
    [SerializeField] 
    [Tooltip("Tiempo que tardo en resetar el sentido del oído.")]
    private float _resetEarSenseSeconds;
    
    [SerializeField] 
    [Tooltip("Tiempo que tardo en patrullar desde que he escuchado al player.")]
    private float _secondsListening;

    private float _secondsListeningSaved;
    public float SecondsListening
    {
        get => _secondsListening;
        set => _secondsListening = value;
    }
    
    [SerializeField] 
    [Tooltip("Tiempo que tardo en ir a perseguir al jugador.")]
    private float _secondsSeeing;
    private float _secondsSeeingSaved;
    public float SecondsSeeing
    {
        get => _secondsSeeing;
        set => _secondsSeeing = value;
    }
    
    private float viewDistance = 5f;
    #endregion

    #region IA
    private FsmEnemyWillOWisp _actualState;

    private NavMeshAgent _navMeshAgent;
    
    private bool _canListen;

    public bool CanListen
    {
        get => _canListen;

        set => _canListen = value;
    }

    private bool _canSee;
    
    public bool CanSee
    {
        get => _canSee;

        set => _canSee = value;
    }
    
    private int _actualWayPoint = 0;
    #endregion
    
    #region REFERENCIAS
    private readonly ContactFilter2D _contactFilter = new ContactFilter2D();

    private Vector2 _direction;

    private Transform _playerTransform;

    public Transform PlayerTransform
    {
        get => _playerTransform;
    }

    private List<Transform> _torchOnListTransform = new List<Transform>();

    public List<Transform> TorchOnList
    {
        get => _torchOnListTransform;
    }

    public Transform PlayerInitialPosition
    {
        get => _playerInitialPosition;
    }

    private bool _canStart = false;

    private bool _followingPlayer = false;
    private bool _canFollow = false;
    public bool CanFollow { get; set; }

    #endregion
    
    #region UNITY METHODS
    void Start()
    {
        PrepareComponent();
    }
    
    void Update()
    {
        if(_canStart)
            _actualState.Execute(this);
    }
    
    private void PrepareComponent()
    {
        //NavMesh
        _navMeshAgent = GetComponent<NavMeshAgent>();

        Init();
    }

    public void Init()
    {
        _secondsListeningSaved = _secondsListening; //  Guardo los segundos configurados por el jugador 
        
        _secondsSeeingSaved = _secondsSeeing; //  Guardo los segundos para la vista 
        
        _canListen = true; //Activo el sentido del oído

        _canSee = true; //Activo el sentido de la vista

        _actualState = new EnemyWillOWispPatrolState(); //Comenzamos con patrulla

        ResetWayPoints();
        
        UpdatePatrolWayPoint(GetNextWayPoint());

        _canStart = true;
    }
    #endregion
    
    #region MOVEMENT & NAVMESH
    
    public void UpdatePatrolWayPoint(Transform waypoint)
    {
        _navMeshAgent.destination = waypoint.position;
    }

    public void Patrol()
    {
        _navMeshAgent.destination = ActualWayPoint().position;
    }
    
    public Transform GetNextWayPoint()
    {
        _actualWayPoint = (_actualWayPoint + 1) % _wayPointsList.Count;
        return _wayPointsList[_actualWayPoint];
    }

    public Transform ActualWayPoint()
    {
        return _wayPointsList[_actualWayPoint];
    }


    public void ResetWayPoints()
    {
        _actualWayPoint = 0;
    }
    
    public void ChangeNavMeshAgentSpeed(float vel)
    {
        _navMeshAgent.speed = vel;
    }
    
    //Persigo Jugador
    public void FollowPlayer()
    {
        _followingPlayer = true;
        
        UpdatePatrolWayPoint(_playerTransform);
    }
    
    // Patrullo por las antorchas
    public void TorchPatrol()
    {
        if (_torchOnListTransform.Count > 0)
        {
            Torch torch = _torchOnListTransform[0].GetComponent<Torch>();
            
            if (torch != null && torch.Activated)
            {
                GameObject torchPatrol = new GameObject();   
                
                torchPatrol.transform.position = new Vector3(_torchOnListTransform[0].position.x,
                    _torchOnListTransform[0].position.y, 0f);

                UpdatePatrolWayPoint(torchPatrol.transform);
                
                SetTorchOff(torchPatrol.transform.position);
                
                Destroy(torchPatrol);
            }
            else
            {
                _torchOnListTransform.RemoveAt(0);
            }
        }
    }

    public void SetTorchOff(Vector3 torchPos)
    {
        Vector3 origin = transform.position;
        Vector2 direction = torchPos - origin;
        float distance = direction.magnitude;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, LayerMask.GetMask(Constants.LAYER_INTERACTABLE));

        if (hit.collider != null)
        {
            Torch torch = hit.collider.GetComponent<Torch>();

            if (torch != null)
            {
                if (Vector3.Distance(transform.position, torch.transform.position) < 1.5f)
                {
                    torch.DeactivateTorch();
                }
            }
        }
    }
    
    public void TorchReset()
    {
        for (int i = 0; i < _torchList.Count; i++)
        {
            _torchList[i].GetComponent<Torch>().Activated = false;
        }
    }

    #endregion
    
    #region SENSES
    //Escucho al jugador?
    public bool ListenPlayer()
    {
        CheckIfFollowed();
        
        bool result = false;
        
        Collider2D[] colliders = new Collider2D[5];

        int objectsDetected = Physics2D.OverlapCircle(transform.position, _sightRadio, _contactFilter, colliders);

        if (objectsDetected > 0)
        {
            foreach (var item in colliders)
            {
                if (item != null)
                {
                    if (item.CompareTag(Constants.TAG_PLAYER) && _canListen)
                    {
                        _playerTransform = item.transform;
                        result = true;
                        break;
                    }
                }
            }
        }
        return result;
    }

    //Veo al jugador?
    public bool SeePlayer()
    {
        CheckIfFollowed();
        
        var result = false;
       
        Collider2D objectDetected = Physics2D.OverlapCircle(transform.position, _sightAware, 
            LayerMask.GetMask(Constants.LAYER_INTERACTABLE, Constants.LAYER_PLAYER));

        if (objectDetected != null)
        {
            if (objectDetected.CompareTag(Constants.TAG_PLAYER))
            {
                _playerTransform = objectDetected.transform;
                result = true;
            }
        }

        return result;
    }
    
    public bool ObstacleDetection()
    {
        var result = false;

        Vector2 direction = _playerTransform.position - transform.position;
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, viewDistance,
            LayerMask.GetMask(Constants.LAYER_INTERACTABLE));

        if (hit.collider != null)
        {
            result = true;
        }
        
        return result;
    }
    
    public bool CheckPlayerDistance()
    {
        if (_playerTransform != null)
            return Vector3.Distance(transform.position, _playerTransform.position) < _playerFollowDistance;
        else
            return false;
    }

    private bool CheckIfFollowed()
    {
        _canFollow = true;
        
        if (_followingPlayer && _canFollow) // Significa que vengo de perseguir al player
        {
            _canFollow = false;
            StartCoroutine(nameof(FollowPlayer));
        }

        return _canFollow;
    }

    private IEnumerator FollowCoolDown()
    {
        yield return new WaitForSeconds(2f);

        _followingPlayer = false;
    }
    //¿Hay antorchas encendidas?
    public bool CheckTorchOn()
    {
        bool result = false;
        
        if (_torchList != null)
        {
            //Compruebo si hay alguna antorcha encendida
            for (int i = 0; i < _torchList.Count; i++)
            {
                if (_torchList[i].GetComponent<Torch>().Activated)
                {
                    if (IsAllTorchOn()) Destroy(gameObject);
                    else
                    {
                        //Guardo las posiciones de las antorchas encendidas
                        _torchOnListTransform.Add(_torchList[i].transform);
                        result = true;   
                    }
                }
            }
            _torchOnListTransform = _torchOnListTransform.OrderBy(t => Vector3.Distance(transform.position, t.position)).ToList();
        }
        return result;
    }
    #endregion    

    #region LOGIC
    public void ChangeState(FsmEnemyWillOWisp newState)
    {
        _actualState = newState;
    }
    
    public void ResetListenTimer()
    {
        StartCoroutine(nameof(WaitUntilResetTimer));
    }
    
    private IEnumerator WaitUntilResetTimer()
    {
        yield return new WaitForSeconds(_resetEarSenseSeconds);
        
        _secondsListening = _secondsListeningSaved;

        _canListen = true;
    }
    
    public void ResetSeeTimer()
    {
        _secondsSeeing = _secondsSeeingSaved;
    }

    public void ResetSeeSense()
    {
        if (_secondsSeeing > 0f)
        {
            _secondsSeeing -= Time.deltaTime;
        }
        else
        {
            _canSee = true;
            ResetSeeTimer();
        }
    }

    public void Reset()
    {
        _canStart = false;
        transform.position = GetNextWayPoint().position;
        StartCoroutine(nameof(WaitUntilReset), _playerTransform);
    }

    private IEnumerator WaitUntilReset(Transform player)
    {
        yield return new WaitForSeconds(_resetSeconds);
        
        player.transform.position = PlayerInitialPosition.position;

        Init();
    }

    //Compruebo si todas las antorchas estan encendidas para matar a willo
    private bool IsAllTorchOn()
    {
        int torchActivatedCounter = 0;
        
        if (_torchList != null)
        {
            for (int i = 0; i < _torchList.Count; i++)
            {
                if (_torchList[i].GetComponent<Torch>().Activated)
                {
                    torchActivatedCounter++;
                }
            }
            
        }
        return _torchList.Count == torchActivatedCounter ? true : false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _sightRadio);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _sightAware);

        // Angulo de vision
        Gizmos.color = Color.yellow;


    }


public void ChangeStatusColor(string state)
{
    switch (state)
    {
        case "Alert":
            ColorUtility.TryParseHtmlString("#FFC400", out Color alertColor); // Amarillo
            //alertColor.a = 50f / 255f;
            _light2D.color = alertColor;
            break;
        case "Danger":
            ColorUtility.TryParseHtmlString("#A91A00", out Color dangerColor); // Rojo
            //dangerColor.a = 50f / 255f;
            _light2D.color = dangerColor;
            break;
        case "Patrol":
            ColorUtility.TryParseHtmlString("#FF00E3", out Color patrolColor); // Morado
            //patrolColor.a =  50f / 255f;
            _light2D.color = patrolColor;
            break;
        case "Torch":
            ColorUtility.TryParseHtmlString("#000EA8", out Color torchColor); // Azul
            //torchColor.a = 50f / 255f;
            _light2D.color = torchColor;
            break;
    }
}



    #endregion
    }
}
