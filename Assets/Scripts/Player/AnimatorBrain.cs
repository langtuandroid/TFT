using System;
using UnityEngine;

public class AnimatorBrain : MonoBehaviour
{
    public event Action OnJumpableHasLanded;

    private Animator _animator;

    private void Start()
    {
        //_animator = GetComponent<Animator>();
        JumpEvents jumpEvents = ServiceLocator.GetService<JumpEvents>();
        jumpEvents.OnJumpStarted  += JumpEvents_OnJumpStarted;
        jumpEvents.OnJumpFinished += JumpEvents_OnJumpFinished;
        jumpEvents.OnJumpableActionStarted += JumpEvents_OnJumpableActionStarted;
    }

    private void JumpEvents_OnJumpableActionStarted()
    {
        _animator.SetTrigger( "OnJumpable" );
    }

    private void JumpEvents_OnJumpFinished()
    {
        _animator.SetBool( Utils.Constants.ANIM_PLAYER_JUMP , false );
    }

    private void JumpEvents_OnJumpStarted()
    {
        _animator.SetBool( Utils.Constants.ANIM_PLAYER_JUMP , true );
    }

    public void HasLandedAfterJumpable()
    {
        OnJumpableHasLanded?.Invoke();
    }

    private void OnDestroy()
    {
        JumpEvents jumpEvents = ServiceLocator.GetService<JumpEvents>();
        jumpEvents.OnJumpStarted  -= JumpEvents_OnJumpStarted;
        jumpEvents.OnJumpFinished -= JumpEvents_OnJumpFinished;
        jumpEvents.OnJumpableActionStarted -= JumpEvents_OnJumpableActionStarted;
    }
}
