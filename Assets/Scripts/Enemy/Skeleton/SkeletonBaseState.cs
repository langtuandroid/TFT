
public abstract class SkeletonBaseState 
{
    protected SkeletonController   _ctx;
    protected SkeletonStateFactory _states;

    public SkeletonBaseState( SkeletonController currentContext , SkeletonStateFactory states )
    {
        _ctx    = currentContext;
        _states = states;
    }
    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    protected void SwitchState( SkeletonBaseState newState )
    {
        _ctx.CurrentState.ExitState();
        newState.EnterState();
        _ctx.CurrentState = newState;
    }
}
