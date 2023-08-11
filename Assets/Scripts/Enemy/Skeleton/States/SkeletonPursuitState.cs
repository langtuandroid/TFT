using UnityEngine;

public class SkeletonPursuitState : SkeletonBaseState
{
    public SkeletonPursuitState( SkeletonController currentContext , SkeletonStateFactory states )
        : base( currentContext , states )
    { }

    private float _secondsToAttack;
    private float _attackTimer;

    public override void EnterState()
    {
        _secondsToAttack = 2;
        _attackTimer = 0;
    }

    public override void UpdateState()
    {
        _attackTimer += Time.deltaTime;
        if ( _attackTimer > _secondsToAttack )
        {
            SwitchState( states.Attack() );
            return;
        }

        ctx.Movement();
    }
}
