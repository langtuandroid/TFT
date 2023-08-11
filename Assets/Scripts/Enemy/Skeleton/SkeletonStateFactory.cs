using System.Collections.Generic;

public class SkeletonStateFactory
{
    private readonly SkeletonController ctx;
    private readonly Dictionary<States, SkeletonBaseState> states = new();

    public enum States
    {
        Idle,
        Alert,
        Attack
    }

    public SkeletonStateFactory( SkeletonController enemyController )
    {
        ctx = enemyController;
        states.Add( States.Idle , new SkeletonIdleState( ctx , this ) );
    }

    public SkeletonBaseState SelectState( States newState ) { return states[newState]; }
}
