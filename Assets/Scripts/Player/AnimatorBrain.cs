using System;
using UnityEngine;

namespace Player
{
    public class AnimatorBrain : MonoBehaviour
    {
        public event Action OnJumpableHasLanded;

        private Animator _animator;

        private const string IDLE = "IdleTree";
        private const string JUMP = "JumpTree";
        private const string JUMPABLE = "OnJumpable";

        private void Start()
        {
            _animator = GetComponent<Animator>();
            Jump jump = GetComponentInParent<Jump>();
            jump.OnJumpStarted  += Jump_OnJumpStarted;
            jump.OnJumpFinished += Jump_OnJumpFinished;
            jump.OnJumpableActionStarted += Jump_OnJumpableActionStarted;
        }

        private void Play( string nameState ) => _animator.Play( nameState );

        private void Jump_OnJumpableActionStarted()
        {
            Play( JUMPABLE );
        }

        private void Jump_OnJumpFinished()
        {
            Play( IDLE );
        }

        private void Jump_OnJumpStarted()
        {
            Play( JUMP );
        }

        public void HasLandedAfterJumpable()
        {
            Play( IDLE );
            OnJumpableHasLanded?.Invoke();
        }
    }
}