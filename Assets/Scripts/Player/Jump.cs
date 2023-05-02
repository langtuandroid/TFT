using UnityEngine;

namespace Player
{
    public class Jump : MonoBehaviour
    {
        [SerializeField] private Transform _playerVisuals;
        [SerializeField][Range(0, 5)] private float _jumpSpeed = 3f;
        [SerializeField][Range(0, 5)] private float _fallSpeed = 4f;
        [SerializeField][Range(0, 2)] private float _maxJumpHeight = 1f;

        private float _yOffset;

        private bool _canJump = true;
        private bool _isJumping = false;

        private float _timer = 0;
        private float _cooldownSeconds = 0.1f;

        private float _z = 0; // jump virtual axis


        private void Awake()
        {
            _yOffset = _playerVisuals.localPosition.y;
            _z = _yOffset;
        }

        public void JumpAction()
        {
            if ( _canJump )
            {
                _isJumping = true;
                _canJump = false;
                // TODO: Change animation to jump
            }

            if ( _isJumping )
            {
                if ( _z < _maxJumpHeight )
                {
                    _z += Time.deltaTime * _jumpSpeed;
                    _z = Mathf.Lerp( _z , _maxJumpHeight , Time.deltaTime * _jumpSpeed );
                    MoveZ();
                }
                else
                    _isJumping = false;
            }
            else
                Fall();
        }

        public void Fall()
        {
            if ( _z > 0 )
            {
                _isJumping = false;
                _z += - Time.deltaTime * _fallSpeed;
            }
            else
            {
                _z = 0;
                CanJumpTimer();
            }

            MoveZ();
        }

        private void MoveZ() => _playerVisuals.localPosition = new Vector3( 0 , _z + _yOffset );

        private void CanJumpTimer()
        {
            if ( _canJump ) return;

            _timer += Time.deltaTime;
            if ( _timer > _cooldownSeconds )
            {
                _timer = 0;
                _canJump = true;
            }
        }

        public bool IsJumping => _isJumping;
        public bool CanJump => _canJump;
    }
}