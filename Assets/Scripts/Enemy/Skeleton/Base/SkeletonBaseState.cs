
public abstract class SkeletonBaseState 
{
    protected SkeletonController   ctx;
    protected SkeletonStateFactory states;

    public SkeletonBaseState( SkeletonController currentContext , SkeletonStateFactory states )
    {
        ctx         = currentContext;
        this.states = states;
    }
    public abstract void EnterState();
    public abstract void UpdateState();
    protected void SwitchState( SkeletonBaseState newState )
    {
        newState.EnterState();
        ctx.CurrentState = newState;
    }
}
