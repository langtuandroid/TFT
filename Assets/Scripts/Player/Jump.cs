// ************ @autor: �lvaro Repiso Romero *************
using System;
using UnityEngine;

namespace Player
{
    public class Jump : MonoBehaviour
    {
        public event Action OnJumpStarted;
        public event Action OnJumpFinished;
        public event Action OnJumpableActionStarted;

        private enum JumpState { Grounded, Jumping, Falling, Cooldown }

        [SerializeField] private Transform _playerVisuals;
        [SerializeField] private LayerMask _jumpableMask;
        [SerializeField] private LayerMask _initialGroundLevelMask;
        private int _currentFloorBitPosition;

        private float _jumpSpeed      = 3f;
        private float _fallSpeed      = 4f;
        private float _maxJumpHeight  = 1f;
        private float _checkRayLength = 0.55f;

        private AudioSpeaker _audioSpeaker;

        private JumpState _jumpState;
        private Timer     _cooldownTimer;
        private float     _yOffset;
        private float     _z = 0; // jump virtual axis

        private IJumpable _jumpable;
        private Timer     _jumpDownTimer;
        private Vector2   _colliderOffset;
        private Vector2   _rayCastOffset = new( 0.2f , 0.2f );

        public void Init()
        {
            _cooldownTimer = new Timer( 0.1f );
            _jumpDownTimer = new Timer( 0.6f );
            _yOffset       = _playerVisuals.localPosition.y;
            _jumpState     = JumpState.Grounded;

            _audioSpeaker   = ServiceLocator.GetService<AudioSpeaker>();
            _colliderOffset = GetComponent<Collider2D>().offset;

            _currentFloorBitPosition = _initialGroundLevelMask.value;
            Debug.Log( _currentFloorBitPosition );

            GetComponentInChildren<AnimatorBrain>().OnJumpableHasLanded += AnimatorBrain_OnJumpableHasLanded;
        }

        public void JumpAction( bool jumpInput , Vector2 lookDirection , Vector2 moveDirection )
        {
            switch ( _jumpState )
            {
                case JumpState.Grounded:

                    if ( jumpInput )
                        StartJump();
                    else
                    if ( moveDirection.magnitude > 0 && moveDirection == lookDirection )
                        CheckJumpDown( lookDirection );
                    else
                        _jumpDownTimer.Restart();

                    break;

                case JumpState.Jumping:

                    CheckJumpable( lookDirection );

                    if ( jumpInput && _z < _maxJumpHeight )
                        JumpAction();
                    else
                        _jumpState = JumpState.Falling;

                    break;

                case JumpState.Falling:

                    if ( _z > 0 )
                        Fall();
                    else
                        Landing();

                    MoveZ();

                    break;

                case JumpState.Cooldown:

                    if ( _cooldownTimer.HasTickForever() )
                    {
                        _jumpState = JumpState.Grounded;
                        OnJumpFinished?.Invoke();
                    }

                    break;
            }
        }


        private void StartJump()
        {
            _jumpState = JumpState.Jumping;
            _audioSpeaker.PlaySound( AudioID.G_PLAYER , AudioID.S_JUMP );
            _jumpDownTimer.Restart();
            OnJumpStarted?.Invoke();
        }

        private void JumpAction()
        {
            _z += Time.deltaTime * _jumpSpeed;
            _z = Mathf.Lerp( _z , _maxJumpHeight , Time.deltaTime * _jumpSpeed );
            MoveZ();
        }

        private void MoveZ()
        {
            _playerVisuals.localPosition = new Vector3( 0 , _z + _yOffset );
        }

        private void Fall()
        {
            _z += -Time.deltaTime * _fallSpeed;

            if ( _z < _maxJumpHeight / 2 )
                JumpGroundUp();
        }

        private void Landing()
        {
            _z = 0;
            _jumpState = JumpState.Cooldown;
            _audioSpeaker.PlaySound( AudioID.G_PLAYER , AudioID.S_LANDING );
        }


        private void CheckJumpDown( Vector2 lookDirection )
        {
            Vector2 origin = new Vector2( _colliderOffset.x + transform.position.x ,
                                          _colliderOffset.y + transform.position.y );

            RaycastHit2D hit = Physics2D.Raycast( origin , lookDirection , _checkRayLength , _currentFloorBitPosition );

            if ( hit )
            {
                Debug.DrawLine( origin , origin + lookDirection * _checkRayLength , Color.yellow );
                if ( _jumpDownTimer.HasTickOnce() )
                {
                    JumpGroundDown( lookDirection );
                    _jumpDownTimer.Restart();
                }
            }
        }

        private void JumpGroundDown( Vector3 lookDirection )
        {
            _currentFloorBitPosition /= 2;
            Debug.Log( _currentFloorBitPosition );
            int directionFactor = lookDirection == Vector3.down ? 2 : 1;
            transform.position += directionFactor * lookDirection;
        }


        private void CheckJumpable( Vector2 lookDirection )
        {
            if ( _z < _maxJumpHeight / 2 ) return;

            float xRayOffset = lookDirection.y != 0 ? _rayCastOffset.x : 0;
            float yRayOffset = lookDirection.x != 0 ? _rayCastOffset.y : 0;


            Vector2 origin = new Vector2( _colliderOffset.x + transform.position.x + xRayOffset,
                                          _colliderOffset.y + transform.position.y + yRayOffset );

            RaycastHit2D hit = Physics2D.Raycast( origin , lookDirection , _checkRayLength , _jumpableMask );

            Debug.DrawLine( origin , origin + lookDirection * _checkRayLength , Color.red );

            if ( hit )
                if ( hit.collider.TryGetComponent( out _jumpable ) )
                {
                    _jumpable.ChangeToJumpable( true );
                    return;
                }


            origin = new Vector2( _colliderOffset.x + transform.position.x - xRayOffset ,
                                  _colliderOffset.y + transform.position.y - yRayOffset );

            hit = Physics2D.Raycast( origin , lookDirection , _checkRayLength , _jumpableMask );

            Debug.DrawLine( origin , origin + lookDirection * _checkRayLength , Color.red );

            if ( hit )
                if ( hit.collider.TryGetComponent( out _jumpable ) )
                    _jumpable.ChangeToJumpable( true );
        }

        private void JumpGroundUp()
        {
            if ( _jumpable == null )
                return;

            if ( CanJumpOnJumpable() )
            { 
                _jumpable.JumpIn( transform );
                _currentFloorBitPosition *= 2;
                Debug.Log(_currentFloorBitPosition );
                OnJumpableActionStarted?.Invoke();
            }

            _jumpable.ChangeToJumpable( false );
            _jumpable = null;
        }

        private bool CanJumpOnJumpable()
        {
            float minJumpableHeight = _maxJumpHeight * 0.4375f; // 1 / 16 upp * 7 pixels = 0.4375
            
            Vector2 origin = new Vector2( _colliderOffset.x + transform.position.x ,
                                          _colliderOffset.y + transform.position.y );
            float radius = 0.05f;

            return _z > minJumpableHeight && Physics2D.CircleCast( origin, radius , Vector2.zero , 0 , _jumpableMask );
        }

        private void AnimatorBrain_OnJumpableHasLanded()
        {
            _jumpState = JumpState.Grounded;

            Vector3 positionDiference = 
                new Vector3(transform.localPosition.x, 
                            transform.localPosition.y - _yOffset, 0 );

            transform.position += positionDiference;
        }

        public bool IsPerformingJump => !_jumpState.Equals( JumpState.Grounded );
        public bool IsOnAir => _jumpState.Equals( JumpState.Jumping ) || _jumpState.Equals( JumpState.Falling );
        public bool IsCooldown => _jumpState.Equals( JumpState.Cooldown );
    }
}