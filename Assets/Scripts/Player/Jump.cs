using UnityEngine;

namespace Player
{
    public class Jump : MonoBehaviour
    {
        [SerializeField] private Transform _playerVisuals;
        [SerializeField][Range(0, 5)] private float _jumpSpeed = 3f;
        [SerializeField][Range(0, 5)] private float _fallSpeed = 4f;
        [SerializeField][Range(0, 2)] private float _maxJumpHeight = 1f;

        private AudioSpeaker _audioSpeaker;
        private float _yOffset;
        private enum JumpState { Grounded, Jumping, Falling, Cooldown }
        private JumpState _jumpState;
        private Timer _cooldownTimer;

        private float _z = 0; // jump virtual axis


        public void Init()
        {
            float _cooldownSeconds = 0.1f;
            _cooldownTimer = new Timer( _cooldownSeconds );
            _audioSpeaker = ServiceLocator.GetService<AudioSpeaker>();
            _yOffset = _playerVisuals.localPosition.y;
            _jumpState = JumpState.Grounded;
        }

        public void JumpAction( bool jumpInput )
        {

            switch ( _jumpState )
            {
            case JumpState.Grounded:
                if ( !jumpInput ) return;
                _jumpState = JumpState.Jumping;
                _audioSpeaker.PlaySound( AudioID.G_PLAYER , AudioID.S_JUMP );
                // TODO: Change animation to jump
                break;

            case JumpState.Jumping:
                if ( jumpInput && _z < _maxJumpHeight )
                {
                    _z += Time.deltaTime * _jumpSpeed;
                    _z = Mathf.Lerp( _z , _maxJumpHeight , Time.deltaTime * _jumpSpeed );
                    MoveZ();
                }
                else _jumpState = JumpState.Falling;
                break;

            case JumpState.Falling:
                if ( _z > 0 )
                    _z += -Time.deltaTime * _fallSpeed;
                else
                {
                    _z = 0;
                    _jumpState = JumpState.Cooldown;
                    _audioSpeaker.PlaySound( AudioID.G_PLAYER , AudioID.S_LANDING );
                }
                MoveZ();
                break;

            case JumpState.Cooldown:
                if ( _cooldownTimer.HasTickForever() )
                    _jumpState = JumpState.Grounded;
                break;
            }
        }

        private void MoveZ() => _playerVisuals.localPosition = new Vector3( 0 , _z + _yOffset );

        public bool IsFalling => _jumpState.Equals( JumpState.Falling );
        public bool IsPerformingJump => !_jumpState.Equals( JumpState.Grounded );
        public bool CanJump => _jumpState.Equals( JumpState.Grounded );
    }
}