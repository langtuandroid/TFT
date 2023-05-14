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

        private const string IDLE = "IdleTree";
        private const string JUMP = "JumpTree";


        private void Start()
        {
            _playerAnimator = GetComponent<Animator>();

            _playerVisualInitialPos = _playerVisuals.localPosition;
            _shadowVisualInitialPos = _shadowVisuals.localPosition;

            Jump jump = GetComponentInParent<Jump>();
            jump.OnJumpStarted  += Jump_OnJumpStarted;
            jump.OnJumpFinished += Jump_OnJumpFinished;
            jump.OnJumpableActionStarted += Jump_OnJumpableActionStarted;
            jump.OnJumpDownStarted += Jump_OnJumpDownStarted;
        }

        private void PlayPlayer( string stateName ) => _playerAnimator.Play( stateName );

        private void Jump_OnJumpDownStarted( Jump.OnJumpDownStartedArgs jumpDownArgs )
        {
            PlayPlayer( JUMP );

            float jumpPower = 2f;
            Debug.Log( jumpDownArgs.landedRelativePosition );
            _playerVisuals.DOLocalJump( jumpDownArgs.landedRelativePosition , jumpPower , 1 , 1 )
                .OnComplete( HasLandedAfterJumpDown_Callback )
                .Play();
        }

        private void HasLandedAfterJumpDown_Callback()
        {
            PlayPlayer( IDLE );

            Vector3 landPosition = new Vector3( _playerVisuals.position.x , _playerVisuals.position.y - 0.8125f );
            Debug.Log( landPosition );
            Debug.Log( _playerVisuals.localPosition );
            Debug.Log( _playerVisuals.position );
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

            float yLandPos = _playerVisuals.localPosition.y - 0.8125f; // Magic number because of DOJump 
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

    }
}