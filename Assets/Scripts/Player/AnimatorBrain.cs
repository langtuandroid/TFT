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
        }

        private void PlayPlayer( string stateName ) => _playerAnimator.Play( stateName );

        private void Jump_OnJumpableActionStarted()
        {
            PlayPlayer( JUMP );

            Vector3 endJumpRelativePos = new Vector3( 0 , 2.5f , 0 );
            float jumpPower = 1;
            _playerVisuals.DOLocalJump( endJumpRelativePos , jumpPower , 1 , 1 )
                .OnComplete( HasLandedAfterJumpable )
                .Play();
            _shadowVisuals.DOLocalMoveY( 2.5f , 0.9f )
                .Play();
        }

        public void HasLandedAfterJumpable()
        {
            PlayPlayer( IDLE );

            float yLandPos = _playerVisuals.localPosition.y - 0.8f; // Magic number from tween
            _playerVisuals.localPosition = _playerVisualInitialPos;
            _shadowVisuals.localPosition = _shadowVisualInitialPos;

            OnJumpableHasLanded?.Invoke( new OnJumpableHasLandedArgs()
            {
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