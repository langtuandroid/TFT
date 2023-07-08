using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.Serialization;
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
    private List<Transform> _wayPointsList;
    
    [SerializeField]
    private List<GameObject> _torchList;
    
    [SerializeField]
    private Transform _playerInitialPosition;
    
    [FormerlySerializedAs("_listenRadio")]
    [SerializeField]
    [Tooltip("Radio del sentido escucha.")]
    private float _sightRadio;
    
    [FormerlySerializedAs("_teleportRatio")]
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
    private int _actualWayPoint = 0; //TODO

    //private List<Torch> _torchScript;
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

    private Vector2 _direction;

    private bool _teleportPlayer;

    public bool TeleporPlayer
    {
        get => _teleportPlayer;
    }

    private bool _isTorchAction;

    private float _teleportRate = 1.5f;

    private Transform _playerTransform;

    public Transform PlayerTransform
    {
        get => _playerTransform;
    }

    public bool IsTorchAction
    {
        get => _isTorchAction;
        set => _isTorchAction = value;
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

    #endregion
    
    #region UNITY METHODS
    void Start()
    {
        PrepareComponent();
    }
    
    void Update()
    {
        _actualState.Execute(this);
    }
    
    private void PrepareComponent()
    {
        //NavMesh
        _navMeshAgent = GetComponent<NavMeshAgent>();

        Init();
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
        Debug.Log("La velocidad ha cambiado: " + vel);
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
                        _playerTransform = item.transform;
                        Debug.Log(Vector3.Distance(transform.position, _playerTransform.position));
                        if (Vector3.Distance(transform.position, _playerTransform.position) < _teleportRate)
                        {
                            _teleportPlayer = true;
                            result = true;
                        }
                        else
                        {
                            _teleportPlayer = false;
                            result = true;
                        }
                            
                        break;
                    }
                }

            }
        }

        return result;
    }

    //Persigo Jugador
    public void FollowPlayer()
    {
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
                Debug.Log("Posicion de la antorcha encendida: " + _torchOnListTransform[0].position);
                UpdatePatrolWayPoint(_torchOnListTransform[0]);
            }
            else
            {
               // _torchOnListTransform.RemoveAt(0);
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
        return PlayerDetection();
    }
    
    public bool CheckPlayerDistance()
    {
        return Vector3.Distance(transform.position, _playerTransform.position) < _playerFollowDistance;
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
                    //Guardo las posiciones de las antorchas encendidas
                    _torchOnListTransform.Add(_torchList[i].GetComponent<Torch>().transform);
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
        StartCoroutine(nameof(WaitUntilReset), _playerTransform);
    }

    private IEnumerator WaitUntilReset(Transform player)
    {
        yield return new WaitForSeconds(_resetSeconds);
        
        player.transform.position = PlayerInitialPosition.position;

        Init();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _sightRadio);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _sightAware);
    }
    #endregion
    }
}
