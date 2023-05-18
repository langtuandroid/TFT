using System;
using UnityEngine;
using DG.Tweening;

namespace Player
{
    public class AnimatorBrain : MonoBehaviour
    {
        public event Action<OnJumpableHasLandedArgs> OnJumpableHasLanded;
        public class OnJumpableHasLandedArgs 
        {
            public float yLandPosition;
        }

        public event Action<OnJumpDownHasLandedArgs> OnJumpDownHasLanded;
        public class OnJumpDownHasLandedArgs
        {
            public Vector3 landedPosition;
        }


        [SerializeField] private Transform _playerVisuals;
        [SerializeField] private Transform _shadowVisuals;

        private Vector3 _playerVisualInitialPos;
        private Vector3 _shadowVisualInitialPos;

        private Animator _playerAnimator;
        private Vector2 _lookDirection;

        [Header("States")]
        private const string IDLE = "IdleTree";
        private const string JUMP = "JumpTree";

        [Header("Parameters")]
        private const string X_DIR = "x";
        private const string Y_DIR = "y";
        private const string IS_WALKING = "IsWalking";


        public void Init()
        {
            _playerAnimator = GetComponent<Animator>();

            _playerVisualInitialPos = _playerVisuals.localPosition;
            _shadowVisualInitialPos = _shadowVisuals.localPosition;

            Jump jump = GetComponentInParent<Jump>();
            jump.OnJumpStarted  += Jump_OnJumpStarted;
            jump.OnJumpFinished += Jump_OnJumpFinished;
            jump.OnJumpableActionStarted += Jump_OnJumpableActionStarted;
            jump.OnJumpDownStarted       += Jump_OnJumpDownStarted;
        }

        private void PlayPlayer( string stateName ) => _playerAnimator.Play( stateName );

        private void Jump_OnJumpDownStarted( Jump.OnJumpDownStartedArgs jumpDownArgs )
        {
            PlayPlayer( JUMP );

            float jumpPower = 2f;
            _playerVisuals.DOLocalJump( jumpDownArgs.landedRelativePosition , jumpPower , 1 , 1 )
                .OnComplete( HasLandedAfterJumpDown_Callback )
                .Play();

            if ( jumpDownArgs.descendDirection == Vector3.up || jumpDownArgs.descendDirection == Vector3.down )
            {
                _shadowVisuals.DOLocalMove( jumpDownArgs.landedRelativePosition , 0.9f )
                    .Play();
            }
            else
            {
                float moveXPixels = 6f / 16;
                Sequence lateralJumpSeq = DOTween.Sequence();
                lateralJumpSeq.Append( _shadowVisuals.DOLocalMoveX( jumpDownArgs.descendDirection.x * moveXPixels , 0.1f  ) );
                lateralJumpSeq.Append( _shadowVisuals.DOLocalMoveY( jumpDownArgs.landedRelativePosition.y , 0f  ) );
                lateralJumpSeq.Append( _shadowVisuals.DOLocalMoveX( jumpDownArgs.landedRelativePosition.x , 0.7f  ) );
                lateralJumpSeq.Play();
            }
            
        }

        private void HasLandedAfterJumpDown_Callback()
        {
            PlayPlayer( IDLE );

            Vector3 landPosition = new Vector3( _playerVisuals.position.x , _playerVisuals.position.y - _playerVisualInitialPos.y ); // rectify relative to world
            _playerVisuals.localPosition = _playerVisualInitialPos;
            _shadowVisuals.localPosition = _shadowVisualInitialPos;

            OnJumpDownHasLanded?.Invoke( new OnJumpDownHasLandedArgs() {
                landedPosition = landPosition
            } );
        }


        private void Jump_OnJumpableActionStarted()
        {
            PlayPlayer( JUMP );

            Vector3 endJumpRelativePos = new Vector3( 0 , 2.5f , 0 );
            float jumpPower = 2;
            _playerVisuals.DOLocalJump( endJumpRelativePos , jumpPower , 1 , 1 )
                .OnComplete( HasLandedAfterJumpable_Callback )
                .Play();
            _shadowVisuals.DOLocalMoveY( 2.5f , 0.9f )
                .Play();
        }

        public void HasLandedAfterJumpable_Callback()
        {
            PlayPlayer( IDLE );

            float yLandPos = _playerVisuals.localPosition.y - _playerVisualInitialPos.y; // rectify relative to world
            _playerVisuals.localPosition = _playerVisualInitialPos;
            _shadowVisuals.localPosition = _shadowVisualInitialPos;

            OnJumpableHasLanded?.Invoke( new OnJumpableHasLandedArgs() {
                yLandPosition = yLandPos
            } );
        }

        private void Jump_OnJumpFinished()
        {
            PlayPlayer( IDLE );
        }

        private void Jump_OnJumpStarted()
        {
            PlayPlayer( JUMP );
        }


        public void IsWalking( bool isWalking )
        {
            _playerAnimator.SetBool( IS_WALKING , isWalking );
        }
        
        public Vector2 LookDirection( Vector2 direction )
        {
            if ( direction.magnitude > 0 )
            {
                float x = direction.x;
                float y = direction.y;

                if ( Mathf.Abs( x ) > Mathf.Abs( y ) )
                {
                    _lookDirection.x = x > 0f ? 1f : -1f;
                    _lookDirection.y = 0f;
                }
                else if ( Mathf.Abs( y ) > Mathf.Abs( x ) )
                {
                    _lookDirection.x = 0f;
                    _lookDirection.y = y > 0f ? 1f : -1f;
                }
                else
                {
                    if ( x == 0f && y == 0f )
                    {
                        _lookDirection.x = x;
                        _lookDirection.y = y;
                    }
                    else if ( Mathf.Abs( _lookDirection.x ) > Mathf.Abs( _lookDirection.y ) )
                    {
                        _lookDirection.x = x > 0f ? 1f : -1f;
                        _lookDirection.y = 0f;
                    }
                    else
                    {
                        _lookDirection.x = 0f;
                        _lookDirection.y = y > 0f ? 1f : -1f;
                    }
                }

                _playerAnimator.SetFloat( X_DIR , _lookDirection.x );
                _playerAnimator.SetFloat( Y_DIR , _lookDirection.y );
            }
            return _lookDirection;
        }

    }
}