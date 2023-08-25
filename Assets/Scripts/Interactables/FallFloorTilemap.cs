using Player;
using UnityEngine;

public class FallFloorTilemap : MonoBehaviour
{
    private Collider2D _pitCollider;
    private bool _isPlayerOnPit;

    private Rigidbody2D _playerRb = null;
    private PlayerController _playerController;
    private Vector2 _playerColOffest;
    private Collider2D _playerCol;

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
                    float yOffset = 0.5f + 1f / 16 * 4;
                    Vector2 centerPosition = new Vector2( transform.position.x , transform.position.y - yOffset );
                    _playerController.Fall( centerPosition );
                }
                else
                {
                    Vector2 direction = transform.position;
                    direction -= playerPos;
                    _playerController.FallGravity( direction );
                }
            }
        }
    }

    private void OnTriggerEnter2D( Collider2D collision )
    {
        _isPlayerOnPit = true;
        if ( _playerController == null )
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
