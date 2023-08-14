using UnityEngine;

public class BoneProjectile : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField]private float _speed = 6;
    private int   _damage;
    private float _timer;
    private Vector2 _direction;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        if ( _timer < 0 )
            gameObject.SetActive( false );
    }

    private void FixedUpdate()
    {
        _rb.MovePosition( _rb.position + Time.deltaTime * _speed * _direction );
    }

    public void Launch( Transform target , int damage )
    {
        _direction = target.position - transform.position;
        _direction.Normalize();
        _damage = damage;
        _timer = 4;
    }

    private void OnCollisionEnter2D( Collision2D collision )
    {
        //collision.gameObject.GetComponent<Player.PlayerStatus>()?.TakeDamage( _damage );
        gameObject.SetActive( false );
    }
}
