using UnityEngine;

public class BoneProjectile : MonoBehaviour
{
    [SerializeField] private float _speed = 6;

    private Rigidbody2D _rb;
    private int   _damage;
    private float _deathTimer;
    private Vector2 _initialDirection;
    private Vector2 _direction;
    private Transform _target;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _deathTimer -= Time.deltaTime;
        if ( _deathTimer < 0 )
            gameObject.SetActive( false );
    }

    private void FixedUpdate()
    {
        var distance = Vector2.Distance( transform.position, _target.position );
        if ( distance < 3 )
        {
            Debug.Log( distance );
            Vector2 centripetideDir = _target.position - transform.position;

            _direction = 5 * _initialDirection + centripetideDir;
            _direction.Normalize();
        }

        _rb.MovePosition( _rb.position + Time.deltaTime * _speed * _direction );
    }

    public void Launch( Transform target , int damage )
    {
        _initialDirection = target.position - transform.position;
        _initialDirection.Normalize();
        _direction = _initialDirection;
        _target = target;
        _damage = damage;
        _deathTimer = 4;
    }

    private void OnCollisionEnter2D( Collision2D collision )
    {
        //collision.gameObject.GetComponent<Player.PlayerStatus>()?.TakeDamage( _damage );
        gameObject.SetActive( false );
    }
}
