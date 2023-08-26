using UnityEngine;

public class BoneProjectile : MonoBehaviour
{
    [SerializeField] private float _speed = 4;

    private Rigidbody2D _rb;
    private int   _damage;
    private float _deathTimer;
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
            Vector2 centripetideDir = _target.position - transform.position;
            var res = Vector3.Cross( _direction , centripetideDir );
            var normalToDirection = Vector2.Perpendicular( _direction );
            var gravityFactor = 0.01f;

            _direction += res.z > 0 ?
                normalToDirection * gravityFactor : // turn left
                -normalToDirection * gravityFactor; // turn right

            _direction.Normalize();
        }

        _rb.MovePosition( _rb.position + Time.deltaTime * _speed * _direction );
    }

    public void Launch( Transform target , int damage )
    {
        _direction = target.position - transform.position;
        _direction.Normalize();
        _target = target;
        _damage = damage;
        _deathTimer = 4;
    }

    private void OnCollisionEnter2D( Collision2D collision )
    {
        collision.gameObject.GetComponent<Player.PlayerStatus>()?.TakeDamage( _damage );
        gameObject.SetActive( false );
    }
}
