using System;
using UnityEngine;
using DG.Tweening;

namespace Player
{
    public class AnimatorBrain : MonoBehaviour
    {
        public event Action OnJumpableHasLanded;

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

            _playerVisuals.DOMoveY( 2 , 1 )
                .SetRelative( true )
                .OnComplete( HasLandedAfterJumpable )
                .Play();

            _shadowVisuals.DOMoveY( 1.5f , 0.8f )
                .SetRelative( true )
                .Play();
        }

        public void HasLandedAfterJumpable()
        {
            PlayPlayer( IDLE );
            _playerVisuals.localPosition = _playerVisualInitialPos;
            _shadowVisuals.localPosition = _shadowVisualInitialPos;
            OnJumpableHasLanded?.Invoke();
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