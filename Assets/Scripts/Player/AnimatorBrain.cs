using System;
using UnityEngine;

namespace Player
{
    public class AnimatorBrain : MonoBehaviour
    {
        public event Action OnJumpableHasLanded;

        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            Jump jump = GetComponentInParent<Jump>();
            jump.OnJumpStarted  += Jump_OnJumpStarted;
            jump.OnJumpFinished += Jump_OnJumpFinished;
            jump.OnJumpableActionStarted += Jump_OnJumpableActionStarted;
        }

        private void Jump_OnJumpableActionStarted()
        {
            _animator.SetTrigger( "OnJumpable" );
        }

        private void Jump_OnJumpFinished()
        {
            _animator.SetBool( Utils.Constants.ANIM_PLAYER_JUMP , false );
        }

        private void Jump_OnJumpStarted()
        {
            _animator.SetBool( Utils.Constants.ANIM_PLAYER_JUMP , true );
        }

        public void HasLandedAfterJumpable()
        {
            Jump_OnJumpFinished();
            OnJumpableHasLanded?.Invoke();
        }
    }
}