using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField]
    private float speed;

    [SerializeField]
    private float wallAware = 0.5f;

    [SerializeField]
    private LayerMask layer;

    [SerializeField]
    private float secondsToChangeDirection;

    [SerializeField]
    private float earRadious;

    [SerializeField]
    private GameObject player;

    //Referencias
    private Rigidbody2D _rigidbody;

    // Variables Privadas
    private bool _facingRight;

    private float _timer;

    private float _speedSaved;

    public ContactFilter2D contactFilter = new ContactFilter2D();

    //IA
    private bool _canPatrol;

    private bool _canFollow;

    private bool _canCheckDistance;

    private float _secondsWaiting = 1f;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        //Maquina de estado
        //_canPatrol = 1 -> patrullo
        //_canPatrol = 0 -> escuho al jugador y espero
        //


        if (_canPatrol)
        {
            PlayerAware();
            Patrol();
        }
        else if (!_canPatrol) {
            if (_canFollow)
            {
                FollowPlayer();
            }
            else if(_canCheckDistance) {
                CheckPlayerDistance();
            } else
            {
                _canPatrol = true;
            }
            
        }


    }

    private void Init()
    {
        if (transform.localScale.x < 0f) _facingRight = false;
        else if (transform.localScale.x > 0f) _facingRight = true;

        //Guardo la velocidad
        _speedSaved = speed;
    }

    //Estado de patrulla
    public void Patrol()
    {
        Vector2 direction = Vector2.right;

        speed = _speedSaved;

        if (CanChangeDirection())
            Flip();

        if (!_facingRight)
            direction = Vector2.left;

        if (Physics2D.Raycast(transform.position, direction, wallAware, layer))
            Flip();
    }

    //Esta de esperar antes de persecución
    public IEnumerator Wait() {
        yield return new WaitForSeconds(_secondsWaiting);

        _canCheckDistance = true;
    }

    //Compruebo si la distancia con el jugador cumple las condiciones para perseguirle
    private void CheckPlayerDistance() {
        _canCheckDistance = false;
        _canFollow = true;
    }

    //Estado de persecución
    public void FollowPlayer()
    {
        //Compruebo si se aleja mucho o se esconde


        Debug.Log("Sigo al jugador");
    }

    private void FixedUpdate()
    {
        if(_canPatrol)
        {
            float horizontalVelocity = speed;

            if (!_facingRight) horizontalVelocity = horizontalVelocity * -1f;

            _rigidbody.velocity = new Vector2(horizontalVelocity, _rigidbody.velocity.y);
        }

        if (_canFollow)
        {
            _rigidbody.velocity = (player.transform.position - transform.position);
        }
  
    }

    private void Flip()
    {
        _timer = 0f;
        _facingRight = !_facingRight;
        float localScaleX = transform.localScale.x;
        localScaleX = localScaleX * -1f;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private bool CanChangeDirection()
    {
        _timer += Time.deltaTime;

        if (_timer >= secondsToChangeDirection) return true;
        else return false;
    }

    //Solo escucho al jugador si estoy idle
    private void PlayerAware()
    {
        Collider2D[] results = new Collider2D[5];

        int objectsDetected = Physics2D.OverlapCircle(transform.position, earRadious, contactFilter, results);

        //_canPatrol = true;

        if (objectsDetected > 0)
        {
            foreach (var item in results)
            {
                if (item != null)
                {
                if (item.CompareTag("Player"))
                {
                    _canPatrol = false;
                    _canCheckDistance = true;
                }
                }

            }
        }
    }


}
