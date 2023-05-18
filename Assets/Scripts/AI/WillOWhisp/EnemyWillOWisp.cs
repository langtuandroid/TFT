using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using Utils;

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
    [Tooltip("Radio del sentido escucha.")]
    private float _listenRadio;
    
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

    #endregion
    
    #region WAYPOINTS
    [Header("WayPoints")]
    private List<Transform> _wayPointsList = new List<Transform>();
    
    private int _actualWayPoint = 0;
    
    private List<GameObject> _torch;
    
    private List<Torch> _torchScript;
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
    #endregion
    
    #region REFERENCIAS
    private readonly ContactFilter2D _contactFilter = new ContactFilter2D();

    //private LayerMask _playerLayer = 0;
    
    private GameObject _player;
    
    public GameObject Player
    {
        get => _player;
    }
    
    private Rigidbody2D _playerRB;

    private Vector2 _direction;

    private bool _isTorchAction;

    public bool IsTorchAction
    {
        get => _isTorchAction;
        set => _isTorchAction = value;
    }
    
    private List<Transform> _torchOnList;
    
    public List<Transform> TorchOnList
    {
        get => _torchOnList;
    }
    
    private Transform _playerInitialPosition;

    public Transform PlayerInitialPosition
    {
        get => _playerInitialPosition;
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
        
        //Torch
        _torch = new List<GameObject>(FindGameObject.AllWithCaseInsensitiveTag(Constants.TAG_TORCH));
        
        if (_torch != null)
        {
            _torchScript = new List<Torch>();
            for (int i = 0; i < _torch.Count; i++)
            {
                Torch torchComponent = _torch[i].GetComponent<Torch>();
                if (torchComponent != null)
                {
                    _torchScript.Add(torchComponent);
                }
            }
        }
        
        //WayPoints
        List<GameObject> wayPointsObjectList = new List<GameObject>(FindGameObject.AllWithCaseInsensitiveTag(Constants.TAG_WAYPOINT));

        foreach (var wayPoint in wayPointsObjectList)
        {
            _wayPointsList.Add(wayPoint.GetComponent<Transform>());   
        }
        
        //Posición Inicial del Player
        _playerInitialPosition = FindGameObject.WithCaseInsensitiveTag(Constants.TAG_PLAYER_INITIAL_POSITION).GetComponent<Transform>();
    }

    private void Init()
    {
        _secondsListeningSaved = _secondsListening; //  Guardo los segundos configurados por el jugador 
        
        _canListen = true; //Activo el sentido del oído
        
        _actualState = new EnemyWillOWispPatrolState(); //Comenzamos con patrulla
        
        UpdatePatrolWayPoint(GetNextWayPoint());
    }
    #endregion
    
    #region MOVEMENT & NAVMESH
    
    //Patrulla waypoints
    public void UpdatePatrolWayPoint(Transform waypoint)
    {
        _navMeshAgent.destination = waypoint.position;
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

    public void ChangeNavMeshAgentSpeed(float vel)
    {
        _navMeshAgent.speed = vel;
    }

    //Método que lanza un raycast para detectar si tenemos delante
    //al jugador o una pared
    public bool PlayerDetection()
    {
        Collider2D[] colliders = new Collider2D[5];

        var result = false;
        
        int objectsDetected = Physics2D.OverlapCircle(transform.position, _sightAware, _contactFilter, colliders);

        if (objectsDetected > 0)
        {
            foreach (var item in colliders)
            {
                if (item != null)
                {
                    if (item.CompareTag(Constants.TAG_PLAYER))
                    {
                        result = true;
                        break;
                    }
                }

            }
        }

        return result;

    }
    
    //Método que devuelve la distancia entre fuego fatuo y jugador
    //Si estoy persiguiendo al jugador y la distancia es menor que la deseada 
    //Sacamos al jugador del nivel
    public bool CheckPlayerDistance()
    {
        return Vector3.Distance(transform.position, _player.transform.position) < _playerFollowDistance;
    }

    //Persigo Jugador
    public void FollowPlayer()
    {
        UpdatePatrolWayPoint(_player.transform);
    }

    
    // Patrullo por las antorchas
    public void TorchPatrol(){
        if (_torchOnList.Count > 0)
        {
            UpdatePatrolWayPoint(_torchOnList[0]);

            // Si llegamos a nuestro destino, cambiamos nuestro destino al siguiente waypoint
            if (Vector3.Distance(transform.position, _torchOnList[0].position) < 1f)
            {
                for (int i = 0; i < _torchScript.Count; i++)
                {
                 if(_torchOnList[0] == _torchScript[i].gameObject.transform)
                     _torchScript[i].Activated = false;
                }
               
                _torchOnList.RemoveAt(0);

                if (_torchOnList.Count > 0)
                {
                    UpdatePatrolWayPoint(_torchOnList[0]);
                }
            }
        }
    }

    public void TorchReset()
    {
        for (int i = 0; i < _torchScript.Count; i++)
        {
            _torchScript[i].Activated = false;
        }
    }

    #endregion
    
    #region SENSES
    //Escucho al jugador?
    public bool ListenPlayer()
    {
        bool result = false;
        
        Collider2D[] colliders = new Collider2D[5];

        int objectsDetected = Physics2D.OverlapCircle(transform.position, _listenRadio, _contactFilter, colliders);

        if (objectsDetected > 0)
        {
            foreach (var item in colliders)
            {
                if (item != null)
                {
                    if (item.CompareTag(Constants.TAG_PLAYER) && _canListen)
                    {
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
        if (Vector3.Distance(transform.position, _player.transform.position) < _sightAware)
        {
                return PlayerDetection();
        }
        else
        {
            return false;
        }
    }
    
    //¿Hay antorchas encendidas?
    public bool CheckTorchOn()
    {
        bool result = false;
        
        _torchOnList = new List<Transform>();
        if (_torch != null)
        {
            //Compruebo si hay alguna antorcha encendida
            for (int i = 0; i < _torchScript.Count; i++)
            {
                if (_torchScript[i].Activated)
                {
                    //Guardo las posiciones de las antorchas encendidas
                    _torchOnList.Add(_torchScript[i].gameObject.transform);
                    result = true;
                }
            }
        }
        return result;
    }
    #endregion    

    #region LOGIC
    public void ChangeState(FsmEnemyWillOWisp newState)
    {
        _actualState = newState;
    }
    
    public void ResetTimer()
    {
        StartCoroutine(nameof(WaitUntilResetTimer));
    }
    
    private IEnumerator WaitUntilResetTimer()
    {
        yield return new WaitForSeconds(_resetEarSenseSeconds);
        
        _secondsListening = _secondsListeningSaved;

        _canListen = true;
    }

    public void Reset()
    {
        StartCoroutine(nameof(WaitUntilReset));
    }

    private IEnumerator WaitUntilReset()
    {
        yield return new WaitForSeconds(_resetSeconds);
        
        _player.transform.position = PlayerInitialPosition.position;

        Init();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _listenRadio);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _sightAware);

        if (_player != null)
        {
            Vector3 playerPos = _player.transform.position;
            Vector3 localPlayerPos = transform.InverseTransformPoint(playerPos);
            Vector3 direction = transform.TransformDirection(localPlayerPos);
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(transform.position, direction);
        }

     


    }
    #endregion
    }
}
