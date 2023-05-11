using System;

public class JumpEvents
{
    public event Action OnJumpStarted;
    public void StartJump() => OnJumpStarted?.Invoke();
    

    public event Action OnJumpFinished;
    public void FinishJump() => OnJumpFinished?.Invoke();
    
    
    public event Action OnJumpableActionStarted;
    public void StartJumpableAction() => OnJumpableActionStarted?.Invoke();
}
