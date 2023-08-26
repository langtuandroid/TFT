
public class SkeletonIdleState : SkeletonBaseState
{
    public SkeletonIdleState( SkeletonController currentContext , SkeletonStateFactory states ) 
        : base( currentContext , states ) { }

    public override void EnterState() { }

    public override void UpdateState()
    {
        if ( ctx.HasDetectPlayer() )
        {
            SwitchState( states.Pursuit() );
            return;
        }

        if ( ctx.CanChangeMoveDirection() )
        {
            ctx.ResetMove();
        }

        ctx.Movement();
    }
}
