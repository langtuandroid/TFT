using Player;
using UnityEngine;

public class BoneProjectile : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Transform   _target;
    [SerializeField]private float _speed = 6;
    private int   _damage;
    private float _timer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 direction = _target.position - transform.position;
        //_rb.MovePosition( _rb.position + Time.deltaTime * _speed * direction.normalized );
        _rb.AddForce( _speed * direction.normalized , ForceMode2D.Force );
        //_rb.velocity = Mathf.Clamp()

        _timer = Time.deltaTime;
        if ( _timer < 0 )
            Destroy( gameObject );
    }

    public void Launch( Transform target , int damage )
    {
        _damage = damage;
        _target = target;
        _timer = 5;
    }

    private void OnCollisionEnter2D( Collision2D collision )
    {
        //collision.gameObject.GetComponent<PlayerStatus>()?.TakeDamage( _damage );
        Destroy( gameObject );
    }
}
