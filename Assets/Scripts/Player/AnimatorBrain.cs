using System;
using UnityEngine;

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
        private Animator _jumpAnimator;

        private const string IDLE = "IdleTree";
        private const string JUMP = "JumpTree";
        private const string JUMP_JUMPABLE = "Jump_Anim_Jumpable_Up";
        private const string JUMP_IDLE = "Jump_Anim_Idle";


        private void Start()
        {
            //_playerAnimator = GetComponent<Animator>();
            _playerAnimator = GetComponentInParent<Animator>();
            //_jumpAnimator = GetComponentInParent<Animator>();

            _playerVisualInitialPos = _playerVisuals.position;
            _shadowVisualInitialPos = _shadowVisuals.position;

            Jump jump = GetComponentInParent<Jump>();
            jump.OnJumpStarted  += Jump_OnJumpStarted;
            jump.OnJumpFinished += Jump_OnJumpFinished;
            jump.OnJumpableActionStarted += Jump_OnJumpableActionStarted;
            jump.OnJumpableActionFinished += Jump_OnJumpableActionFinished;
        }

        private void Jump_OnJumpableActionFinished()
        {
            Debug.Log( "a" );
            PlayPlayer( IDLE );
            Debug.Log( "b" );
            //PlayJumpAnim( JUMP_IDLE );
            Debug.Log( "c" );
            _playerVisuals.localPosition = _playerVisualInitialPos;
            _shadowVisuals.localPosition = _shadowVisualInitialPos;
            //OnJumpableHasLanded?.Invoke();
        }

        private void PlayPlayer( string stateName ) => _playerAnimator.Play( stateName );
        private void PlayJumpAnim( string stateName ) => _jumpAnimator.Play( stateName );

        private void Jump_OnJumpableActionStarted()
        {
            PlayPlayer( JUMP_JUMPABLE );
            //PlayJumpAnim( JUMP_JUMPABLE );
        }

        private void Jump_OnJumpFinished()
        {
            PlayPlayer( IDLE );
        }

        private void Jump_OnJumpStarted()
        {
            PlayPlayer( JUMP );
        }

        public void HasLandedAfterJumpable()
        {
            Debug.Log( "a" );
            PlayPlayer( IDLE );
            Debug.Log( "b" );
            //PlayJumpAnim( JUMP_IDLE );
            Debug.Log( "c" );
            _playerVisuals.localPosition = _playerVisualInitialPos;
            _shadowVisuals.localPosition = _shadowVisualInitialPos;
            OnJumpableHasLanded?.Invoke();
        }
    }
}