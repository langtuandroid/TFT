// ************ @autor: Álvaro Repiso Romero *************
using System;
using UnityEngine;

namespace Player
{
    public class Jump : MonoBehaviour
    {
        [SerializeField] private Transform _playerVisuals;
        [SerializeField][Range(0, 5)] private float _jumpSpeed = 3f;
        [SerializeField][Range(0, 5)] private float _fallSpeed = 4f;
        [SerializeField][Range(0, 2)] private float _maxJumpHeight = 1f;
        [SerializeField] private LayerMask _jumpableMask;

        private AudioSpeaker _audioSpeaker;
        private enum JumpState { Grounded, Jumping, Falling, Cooldown }
        private JumpState _jumpState;
        private Timer _cooldownTimer;
        private float _yOffset;
        private float _z = 0; // jump virtual axis


        private Mushroom _jumpable;
        private Vector2 _colliderOffset;
        private Vector2 _rayCastOffset = new( 0.2f , 0.2f );

        public void Init()
        {
            float _cooldownSeconds = 0.1f;
            _cooldownTimer = new Timer( _cooldownSeconds );
            _audioSpeaker = ServiceLocator.GetService<AudioSpeaker>();
            _yOffset = _playerVisuals.localPosition.y;
            _jumpState = JumpState.Grounded;

            _colliderOffset = GetComponent<Collider2D>().offset;
        }

        public void JumpAction( bool jumpInput , Vector2 lookDirection )
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

                CheckJumpuable( lookDirection );

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
                {
                    _z += -Time.deltaTime * _fallSpeed;
                    if ( _z < _maxJumpHeight / 2 )
                        JumpLevel();
                }
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

        private void JumpLevel()
        {
            _jumpable?.JumpIn( transform );
            _jumpable = null;
        }

        private void CheckJumpuable( Vector2 lookDirection )
        {
            if ( _z < _maxJumpHeight / 2 ) return;

            float xRayOffset = lookDirection.y != 0 ? _rayCastOffset.x : 0;
            float yRayOffset = lookDirection.x != 0 ? _rayCastOffset.y : 0;


            Vector2 origin = new Vector2( _colliderOffset.x + transform.position.x + xRayOffset,
                                      _colliderOffset.y + transform.position.y + yRayOffset );

            RaycastHit2D hit = Physics2D.Raycast( origin , lookDirection , 0.6f , _jumpableMask );

            if ( hit )
            {
                _jumpable = hit.collider.GetComponent<Mushroom>();
                _jumpable.CanBeJump();
                return;
            }


            origin = new Vector2( _colliderOffset.x + transform.position.x - xRayOffset ,
                                  _colliderOffset.y + transform.position.y - yRayOffset );

            hit = Physics2D.Raycast( origin , lookDirection , 0.6f , _jumpableMask );

            if ( hit )
            {
                _jumpable = hit.collider.GetComponent<Mushroom>();
                _jumpable.CanBeJump();
            }
        }

        private void MoveZ() => _playerVisuals.localPosition = new Vector3( 0 , _z + _yOffset );

        public bool IsGrounded => _jumpState.Equals( JumpState.Grounded );
        public bool IsFalling => _jumpState.Equals( JumpState.Falling );
        public bool IsPerformingJump => !_jumpState.Equals( JumpState.Grounded );
        public bool CanJump => _jumpState.Equals( JumpState.Grounded );
    }
}