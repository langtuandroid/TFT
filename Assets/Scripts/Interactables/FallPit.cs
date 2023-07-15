using Player;
using UnityEngine;

public class FallPit : MonoBehaviour
{
    [SerializeField] private GameObject _notBrokenObj;
    private float _gravityForce = 2.5f;

    private Collider2D _pitCollider;
    private Timer _breakTimer;
    private bool _isPlayerOnPit;
    private bool _isBroken;

    private Rigidbody2D _playerRb;
    private PlayerController _playerController;
    private Vector2 _playerColOffest;

    private void Awake()
    {
        _breakTimer = new Timer( 1f );
        _pitCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if ( _isPlayerOnPit )
        { 
            if ( _isBroken )
            {
                if( _playerController.IsGrounded )
                {
                    Vector2 playerPos = _playerRb.position + _playerColOffest;                    

                    if ( _pitCollider.OverlapPoint( playerPos ) )
                    {
                        float yOffset = 0.5f + 1f / 16 * 4;
                        Vector2 centerPosition = new Vector2( transform.position.x , transform.position.y - yOffset );
                        _playerRb.position = centerPosition;
                        _playerController.Fall();
                    }
                    else
                    {
                        Vector2 direction = new Vector2( transform.position.x , transform.position.y ) - playerPos;
                        _playerRb.AddForce( direction.normalized * _gravityForce , ForceMode2D.Force );
                    }
                }
            }
            else
            if ( _breakTimer.HasTickOnce() )
            {
                _isBroken = true;
                _notBrokenObj.SetActive( false );
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
        if ( !_isBroken ) _breakTimer.Restart();
    }
}
