
public class SkeletonPursuitState : SkeletonBaseState
{
    public SkeletonPursuitState( SkeletonController currentContext , SkeletonStateFactory states )
        : base( currentContext , states ) { }

    public override void EnterState() { }

    public override void UpdateState()
    {
        if ( ctx.IsReadyToAttack() )
        {
            SwitchState( states.Attack() );
            return;
        }

        if ( ctx.CanChangeAction() )
        {
            ctx.ResetAction();
        }

        ctx.Movement();
    }
}
