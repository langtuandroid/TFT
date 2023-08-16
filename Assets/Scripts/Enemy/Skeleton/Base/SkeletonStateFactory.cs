using System.Collections.Generic;

public class SkeletonStateFactory
{
    private readonly Dictionary<States, SkeletonBaseState> states = new();

    private enum States
    {
        Idle,
        Pursuit,
        Attack
    }

    public SkeletonStateFactory( SkeletonController enemyController )
    {
        states.Add( States.Idle    , new SkeletonIdleState(    enemyController , this ) );
        states.Add( States.Pursuit , new SkeletonPursuitState( enemyController , this ) );
        states.Add( States.Attack  , new SkeletonAttackState(  enemyController , this ) );
    }

    public SkeletonBaseState Idle()    => states[States.Idle];
    public SkeletonBaseState Pursuit() => states[States.Pursuit];
    public SkeletonBaseState Attack()  => states[States.Attack];
}
