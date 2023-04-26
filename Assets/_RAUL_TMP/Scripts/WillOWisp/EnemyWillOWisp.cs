using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class EnemyWillOWisp : MonoBehaviour
{
    #region CONFIGURATION
    [Header("Configuration")]
    [SerializeField]
    private ContactFilter2D contactFilter = new ContactFilter2D();
    
    [SerializeField]
    private LayerMask playerLayer;

    [SerializeField]
    private float listenRadio;
    
    [SerializeField]
    private float playerFollowDistance;
    
    [SerializeField]
    private float sightAware;

    [SerializeField] private float resetSeconds;
    
    [SerializeField] private float resetEarSenseSeconds;

    [SerializeField]
    private Transform playerInitialPosition;

    public Transform PlayerInitialPosition
    {
        get => playerInitialPosition;
    }
    
    [SerializeField] private float secondsListening;

    private float secondsListeningSaved;
    public float SecondsListening
    {
        get => secondsListening;
        set => secondsListening = value;
    }

    #endregion
    
    #region WAYPOINTS
    [Header("WayPoints")]
    public Transform wayPoint1;
    public Transform wayPoint2;
    public Transform wayPoint3;
    public Transform wayPoint4;
    public int actualWayPoint;
    private GameObject[] torch;
    //private List<Torch> torchScript;
    #endregion
    
    #region IA
    private FsmEnemyWillOWisp actualState;

    public NavMeshAgent _navMeshAgent;

    public bool playerCollision;

    [HideInInspector]
    public bool canListen;
    #endregion
    
    #region REFERENCIAS
    [HideInInspector]
    public Rigidbody2D rb2D;
    
    [HideInInspector]
    public bool facingRight;
    
    [HideInInspector]
    public GameObject player;
    
    private Rigidbody2D playerRB;

    private Vector2 direction;

    [HideInInspector] public bool isTorchAction;

    [HideInInspector] public List<Transform> torchOnList;
    
    #endregion
    
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        playerRB = player.GetComponent<Rigidbody2D>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        torch = GameObject.FindGameObjectsWithTag("Torch");
        /*if (torch != null)
        {
            torchScript = new List<Torch>();
            for (int i = 0; i < torch.Length; i++)
            {
                Torch torchComponent = torch[i].GetComponent<Torch>();
                if (torchComponent != null)
                {
                    torchScript.Add(torchComponent);
                }
            }
        }*/

    
    }

    void Start()
    {
        Init();
    }
    
    void Update()
    {
        actualState.Execute(this);
    }

    private void Init()
    {
        if (transform.localScale.x < 0f) facingRight = false;
        else if (transform.localScale.x > 0f) facingRight = true;

        secondsListeningSaved = secondsListening; //  Guardo los segundos configurados por el jugador 
        
        canListen = true; //Activo el sentido del oído
        
        actualState = new EnemyWillOWispPatrolState(); //Comenzamos con patrulla
    }

    #region MOVEMENT
    
    //Patrulla waypoints
    public void UpdatePatrolWayPoint(Transform waypoint)
    {
        _navMeshAgent.destination = waypoint.position;
    }

    //Está el jugadorn moviendose?
    public bool IsPlayerMoving()
    {
        return playerRB.velocity.magnitude > 0f;
    }

    //Método que lanza un raycast para detectar si tenemos delante
    //al jugador o una pared
    public bool PlayerDetection()
    {
        Vector3 playerPos = player.transform.position;
        Vector3 localPlayerPos = transform.InverseTransformPoint(playerPos); //Necesario debido al navmesh y la rotacion del eje x
        Vector3 direction = transform.TransformDirection(localPlayerPos);
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, playerLayer);

        
        return hit.collider.CompareTag("Player");

    }
    
    //Método que devuelve la distancia entre fuego fatuo y jugador
    //Si estoy persiguiendo al jugador y la distancia es menor que la deseada 
    //Sacamos al jugador del nivel
    public bool CheckPlayerDistance()
    {
        return Vector3.Distance(transform.position, player.transform.position) < playerFollowDistance;
    }

    //Persigo Jugador
    public void FollowPlayer()
    {
        UpdatePatrolWayPoint(player.transform);
    }
    
    // Volteo el sprite si cambio de dirección
    public void Flip()
    {
        facingRight = !facingRight;
        float localScaleX = transform.localScale.x;
        localScaleX = localScaleX * -1f;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }
    
    /*
    // Patrullo por las antorchas
    public void TorchPatrol(){
        if (torchOnList.Count > 0)
        {
            UpdatePatrolWayPoint(torchOnList[0]);

            // Si llegamos a nuestro destino, cambiamos nuestro destino al siguiente waypoint
            if (Vector3.Distance(transform.position, torchOnList[0].position) < 1f)
            {
                for (int i = 0; i < torchScript.Count; i++)
                {
                 if(torchOnList[0] == torchScript[i].gameObject.transform)
                     torchScript[i].Activated = false;
                }
               
                torchOnList.RemoveAt(0);

                if (torchOnList.Count > 0)
                {
                    UpdatePatrolWayPoint(torchOnList[0]);
                }
            }
        }
    }

    public void TorchReset()
    {
        for (int i = 0; i < torchScript.Count; i++)
        {
            torchScript[i].Activated = false;
        }
    }
*/
    #endregion
    
    #region SENSES
    //Escucho al jugador?
    public bool ListenPlayer()
    {
        bool result = false;
        
        Collider2D[] colliders = new Collider2D[5];

        int objectsDetected = Physics2D.OverlapCircle(transform.position, listenRadio, contactFilter, colliders);

        if (objectsDetected > 0)
        {
            foreach (var item in colliders)
            {
                if (item != null)
                {
                    if (item.CompareTag("Player") && canListen)
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
        if (Vector3.Distance(transform.position, player.transform.position) < sightAware)
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
        
        torchOnList = new List<Transform>();
        if (torch != null)
        {
            /*
            //Compruebo si hay alguna antorcha encendida
            for (int i = 0; i < torchScript.Count; i++)
            {
                if (torchScript[i].Activated)
                {
                    //Guardo las posiciones de las antorchas encendidas
                    torchOnList.Add(torchScript[i].gameObject.transform);
                    result = true;
                }
         
            }*/
        }
        return result;
    }
    #endregion    

    #region LOGIC
    public void ChangeState(FsmEnemyWillOWisp newState)
    {
        actualState = newState;
    }
    
    public void ResetTimer()
    {
        StartCoroutine(nameof(WaitUntilResetTimer));
    }
    
    private IEnumerator WaitUntilResetTimer()
    {
        yield return new WaitForSeconds(resetEarSenseSeconds);
        
        secondsListening = secondsListeningSaved;

        canListen = true;
    }

    public void Reset()
    {
        StartCoroutine(nameof(WaitUntilReset));
    }

    private IEnumerator WaitUntilReset()
    {
        yield return new WaitForSeconds(resetSeconds);
        
        player.transform.position = PlayerInitialPosition.position;
        
        actualWayPoint = 1;
        
        ChangeState(new EnemyWillOWispPatrolState());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, listenRadio);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightAware);
        
        Vector3 playerPos = player.transform.position;
        Vector3 localPlayerPos = transform.InverseTransformPoint(playerPos);
        Vector3 direction = transform.TransformDirection(localPlayerPos);

        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, direction);

    }
    #endregion
}