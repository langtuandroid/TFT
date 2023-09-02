// ************ @autor: Álvaro Repiso Romero *************
using System;
using UnityEngine;
using DG.Tweening;

namespace Player
{
    public class Jump
    {
        public event Action OnJumpStarted;
        public event Action OnJumpFinished;
        public event Action OnJumpableActionStarted;
        public event Action<OnJumpDownStartedArgs> OnJumpDownStarted;
        public class OnJumpDownStartedArgs
        {
            public int numFloorsDescended;
            public Vector3 descendDirection;
            public Vector3 landedRelativePosition;
        }


        private enum JumpState { Grounded, Jumping, Falling, Cooldown , Jumpable }

        private Transform _transform;
        private Transform _playerVisuals;
        private LayerMask _boundsMask;

        private LayerMask _jumpableMask;
        private int _initialFloorBitPos;
        private int _currentFloorBitPosition;

        private float _jumpSpeed      = 3f;
        private float _fallSpeed      = 4f;
        private float _maxJumpHeight  = 1f;
        private float _checkRayLength = 0.55f;

        private IAudioSpeaker _audioSpeaker;

        private JumpState _jumpState;
        private Timer     _cooldownTimer;
        private float     _yOffset;
        private float     _z = 0; // jump virtual axis

        private IJumpable _jumpable;
        private Timer     _jumpDownTimer;
        private Vector2   _colliderOffset;
        private Vector2   _rayCastOffset = new( 0.2f , 0.2f );

        public Jump( Vector2 colliderOffset , Transform transform , Transform playerVisuals , 
            PlayerPhysicalDataSO physicalData )
        {
            _transform     = transform;
            _playerVisuals = playerVisuals;

            _cooldownTimer = new Timer( 0.1f );
            _jumpDownTimer = new Timer( 0.6f );
            _yOffset   = _playerVisuals.localPosition.y;
            _jumpState = JumpState.Grounded;

            _colliderOffset = colliderOffset;
            _jumpableMask   = physicalData.interactableLayerMask;
            _boundsMask     = physicalData.boundsLayerMask;
        }

        public void Init( AnimatorBrain animatorBrain , IAudioSpeaker audioSpeaker , LayerMask initialGroundLayerMask )
        {
            _audioSpeaker = audioSpeaker;
            _initialFloorBitPos = initialGroundLayerMask.value;
            _currentFloorBitPosition = _initialFloorBitPos;

            animatorBrain.OnJumpableHasLanded += AnimatorBrain_OnJumpableHasLanded;
            animatorBrain.OnJumpDownHasLanded += Jump_OnJumpDownHasLanded;
        }

        public void FallInHole()
        {
            _jumpState = JumpState.Grounded;
            _currentFloorBitPosition = _initialFloorBitPos;
        }
        float _lastZ;
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

                    if ( jumpInput )
                    {
                        if ( _z < _maxJumpHeight )
                        {
                            JumpAction();
                            _lastZ = _z;
                        }
                        else
                            _jumpState = JumpState.Falling;
                    }
                    else
                    {
                        var minJumpRatio = 0.5f;
                        var middleJumpRatio = 0.75f;
                        if ( _lastZ < _maxJumpHeight * minJumpRatio )
                        {
                            if ( _z < _maxJumpHeight * minJumpRatio )
                                JumpAction();
                            else
                                _jumpState = JumpState.Falling;
                        }
                        else 
                        if ( _lastZ < _maxJumpHeight * middleJumpRatio )
                        {
                            if ( _z < _maxJumpHeight * middleJumpRatio )
                                JumpAction();
                            else
                                _jumpState = JumpState.Falling;
                        }
                        else
                        {
                            if ( _z < _maxJumpHeight )
                                JumpAction();
                            else
                                _jumpState = JumpState.Falling;
                        }
                    }

                    break;

                case JumpState.Falling:

                    if ( _z > 0 )
                        Fall();
                    else
                        Landing();

                    MoveZ();

                    break;

                case JumpState.Cooldown:

                    if ( _cooldownTimer.HasTickOnce() )
                    {
                        _jumpState = JumpState.Grounded;
                        _cooldownTimer.Restart();
                        OnJumpFinished?.Invoke();
                    }

                    break;
            }
        }


        private void StartJump()
        {
            _lastZ = 0;
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
            var origin = new Vector2( _colliderOffset.x + _transform.position.x ,
                                      _colliderOffset.y + _transform.position.y );

            var hit = Physics2D.Raycast( origin , lookDirection , _checkRayLength , _currentFloorBitPosition );

            if ( hit )
            {
                if ( _jumpDownTimer.HasTickOnce() )
                {
                    JumpGroundDown( lookDirection );
                    _jumpDownTimer.Restart();
                }
            }
        }

        private void JumpGroundDown( Vector3 lookDirection )
        {
            int numOfFloors = 1;
            var posToCheck = new Vector3();
            var relativePos = new Vector3();
            if ( lookDirection == Vector3.down )
            {
                float maxNumOfFloors = 3;
                for ( int i = numOfFloors; i <= maxNumOfFloors; i++ )
                {
                    posToCheck = _transform.position + Vector3.down * i;

                    if ( Physics2D.OverlapPoint( posToCheck , _boundsMask ) )
                        return;

                    if ( !Physics2D.OverlapPoint( posToCheck , _currentFloorBitPosition ) )
                    {
                        relativePos = Vector3.down * ( i + 1 );
                        numOfFloors = i;
                        break;
                    }
                }
                posToCheck += Vector3.down;                
            }
            else if ( lookDirection == Vector3.up )
            {
                var dist = 1.5f;
                posToCheck = _transform.position + Vector3.up * dist;
                relativePos = Vector3.up * dist;
            }
            else
            {
                var dist = 1.5f;
                posToCheck = _transform.position + lookDirection * dist + Vector3.down;
                relativePos = lookDirection * dist + Vector3.down;

                dist = 1;
                if ( Physics2D.Raycast( posToCheck , Vector2.up , dist , _currentFloorBitPosition ) ) 
                    return;
            }

            if ( Physics2D.OverlapPoint( posToCheck , _boundsMask ) )
                return;

            _jumpState = JumpState.Jumpable;

            _currentFloorBitPosition /= 2 * numOfFloors;

            OnJumpDownStarted?.Invoke( new OnJumpDownStartedArgs() { 
                numFloorsDescended = numOfFloors,
                descendDirection = lookDirection,
                landedRelativePosition = relativePos
            } );
        }

        private void Jump_OnJumpDownHasLanded( AnimatorBrain.OnJumpDownHasLandedArgs jumpDownLandArgs )
        {
            _jumpState = JumpState.Grounded;
            _transform.position = jumpDownLandArgs.landedPosition;
        }

        private void CheckJumpable( Vector2 lookDirection )
        {
            if ( _z < _maxJumpHeight / 2 ) return;

            float xRayOffset = lookDirection.y != 0 ? _rayCastOffset.x : 0;
            float yRayOffset = lookDirection.x != 0 ? _rayCastOffset.y : 0;


            var origin = new Vector2( _colliderOffset.x + _transform.position.x + xRayOffset,
                                      _colliderOffset.y + _transform.position.y + yRayOffset );

            var hit = Physics2D.Raycast( origin , lookDirection , _checkRayLength , _jumpableMask );

            if ( hit )
                if ( hit.collider.TryGetComponent( out _jumpable ) )
                {
                    _jumpable.ChangeToJumpable( true );
                    return;
                }


            origin = new Vector2( _colliderOffset.x + _transform.position.x - xRayOffset ,
                                  _colliderOffset.y + _transform.position.y - yRayOffset );

            hit = Physics2D.Raycast( origin , lookDirection , _checkRayLength , _jumpableMask );

            if ( hit )
                if ( hit.collider.TryGetComponent( out _jumpable ) )
                    _jumpable.ChangeToJumpable( true );
        }

        private void JumpGroundUp()
        {
            if ( _jumpable == null )
                return;

            if ( CanJumpOnJumpable( out Vector3 jumpablePos ) )
            {
                _jumpState = JumpState.Jumpable;
                _jumpable.JumpIn();

                _transform.DOMove( jumpablePos , 0.5f )
                    .SetRelative( false )
                    .SetEase( Ease.Linear )
                    .SetLoops( 1 )
                    .Play();

                OnJumpableActionStarted?.Invoke();
            }

            _jumpable.ChangeToJumpable( false );
            _jumpable = null;
        }

        private bool CanJumpOnJumpable( out Vector3 jumpablePos )
        {
            jumpablePos = Vector3.zero;
            var minJumpableHeight = _maxJumpHeight * 0.4375f; // 1 / 16 upp * 7 pixels = 0.4375
            
            var origin = new Vector2( _colliderOffset.x + _transform.position.x ,
                                      _colliderOffset.y + _transform.position.y );
            var radius = 0.05f;
            var hit = Physics2D.CircleCast( origin, radius , Vector2.zero , 0 , _jumpableMask );
            if ( hit )
                jumpablePos = hit.collider.GetComponent<Transform>().position +
                            new Vector3( hit.collider.offset.x , hit.collider.offset.y );

            return _z > minJumpableHeight && hit;
        }

        private void AnimatorBrain_OnJumpableHasLanded( AnimatorBrain.OnJumpableHasLandedArgs onJumpableHasLanded )
        {
            _jumpState = JumpState.Grounded;
            _currentFloorBitPosition *= 2;
            _transform.position += new Vector3( 0 , onJumpableHasLanded.yLandPosition , 0);
        }

        public bool IsPerformingJump => !_jumpState.Equals( JumpState.Grounded );
        public bool IsOnAir => _jumpState.Equals( JumpState.Jumping ) || _jumpState.Equals( JumpState.Falling );
        public bool IsCooldown => _jumpState.Equals( JumpState.Cooldown );
    }
}