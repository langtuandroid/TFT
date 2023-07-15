using Player;
using UnityEngine;

public class FallFloorTilemap : MonoBehaviour
{
    [SerializeField] private Transform _centerOfHole;

    private Collider2D _pitCollider;
    private float _gravityForce = 2.5f;
    private bool _isPlayerOnPit;

    private Rigidbody2D _playerRb;
    private PlayerController _playerController;
    private Vector2 _playerColOffest;

    private void Awake()
    {
        _pitCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if ( _isPlayerOnPit )
        {
            if ( _playerController.IsGrounded )
            {
                Vector2 playerPos = _playerRb.position + _playerColOffest;

                if ( _pitCollider.OverlapPoint( playerPos ) )
                {
                    _playerController.Fall();
                }
                else
                {
                    Vector2 direction = _centerOfHole.position;
                    direction -= playerPos;
                    //_playerRb.AddForce( direction.normalized * _gravityForce , ForceMode2D.Force );
                }
            }
        }
    }

    private void OnTriggerEnter2D( Collider2D collision )
    {
        _isPlayerOnPit = true;
        if ( _playerRb.Equals( null ) )
        {
            _playerRb = collision.GetComponent<Rigidbody2D>();
            _playerColOffest = _playerRb.GetComponent<Collider2D>().offset;
            _playerController = _playerRb.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerExit2D( Collider2D collision )
    {
        _isPlayerOnPit = false;
    }
}
