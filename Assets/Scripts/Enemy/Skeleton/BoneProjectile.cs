using Player;
using UnityEngine;

public class BoneProjectile : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Transform _target;
    private int _damage;
    private float _speed;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 direction = _target.position - transform.position;
        _rb.MovePosition( _rb.position + Time.deltaTime * _speed * direction );
    }

    public void Launch( Transform target , int damage )
    {
        _damage = damage;
        _target = target;
    }

    private void OnCollisionEnter2D( Collision2D collision )
    {
        collision.gameObject.GetComponent<PlayerStatus>().TakeDamage( _damage );
    }
}
