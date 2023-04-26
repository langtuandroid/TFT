using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySlime : MonoBehaviour
{
    [Header("Configuration")]
    public float speed;

    [SerializeField]
    private float wallAware = 0.5f;

    [SerializeField]
    private LayerMask layer;

    [SerializeField]
    private float secondsToChangeDirection;

    [SerializeField]
    private float earRadious;
    
    [SerializeField]
    private ContactFilter2D contactFilter = new ContactFilter2D();

    //Referencias
    [HideInInspector]
    public Rigidbody2D rb2D;
    
    [HideInInspector]
    public GameObject player;
    
    [HideInInspector]
    public bool facingRight;
    //TODO getter setter

    private float _timer;

    //IA
    private bool _canPatrol;

    private bool _canFollow;

    private bool _canCheckDistance;

    private bool _canSeePlayer;
    
    private NavMeshAgent _navMeshAgent;

    [HideInInspector]
    public FsmEnemySlime actualState;
    
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }
    
    void Start()
    {
        Init();
    }
    
    void Update()
    {
        actualState.Execute(this);
    }

    public void ChangeState(FsmEnemySlime newState)
    {
        actualState = newState;
    }
    
    private void Init()
    {
        if (transform.localScale.x < 0f) facingRight = false;
        else if (transform.localScale.x > 0f) facingRight = true;

        actualState =  new EnemySlimePatrolState();
    }
    
    #region SENSES

    public bool CanSeePlayer()
    {
        _canSeePlayer = false;
        
        Collider2D[] results = new Collider2D[5];

        int objectsDetected = Physics2D.OverlapCircle(transform.position, earRadious, contactFilter, results);

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

        if (_timer >= secondsToChangeDirection) return true;
        else return false;
    }
    
    public bool ObstacleAware()
    {
        bool result = false;
        
        Vector3 direction = transform.TransformDirection(player.transform.position - transform.position);

        if (Physics2D.Raycast(transform.position, direction, wallAware, layer))
            result = true;
        
        return result;
 
    }
    
    #endregion
    
    #region MOVEMENT
    public void Flip()
    {
        _timer = 0f;
        facingRight = !facingRight;
        float localScaleX = transform.localScale.x;
        localScaleX = localScaleX * -1f;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    public void Patrol()
    {

    }

    public void Follow()
    {
        _navMeshAgent.destination = player.transform.position;
    }
    
    public void PatrolDirection()
    {
        Vector2 direction = Vector2.right;

        if (CanChangeDirection())
            Flip();

        if (!facingRight)
            direction = Vector2.left;

        if (Physics2D.Raycast(transform.position, direction, wallAware, layer))
            Flip();
    }

   #endregion
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, earRadious);

        Gizmos.color = Color.blue;
        Vector3 direction = transform.TransformDirection(player.transform.position - transform.position);
        Gizmos.DrawRay(transform.position, direction);
        
        Gizmos.color = Color.green;
        Vector3 direction2 = transform.TransformDirection(player.transform.position - transform.position);
        Gizmos.DrawRay(transform.position, direction2 * 1.5f);

    }
}
