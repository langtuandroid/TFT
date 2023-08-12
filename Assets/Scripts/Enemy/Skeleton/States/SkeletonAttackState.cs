
public class SkeletonAttackState : SkeletonBaseState
{
    public SkeletonAttackState( SkeletonController currentContext , SkeletonStateFactory states )
        : base( currentContext , states ) { }

    public override void EnterState()
    {
        ctx.Attack();
    }

    public override void UpdateState()
    {
        SwitchState( ctx.HasDetectPlayer() ? states.Pursuit() : states.Idle() );
    }
}
