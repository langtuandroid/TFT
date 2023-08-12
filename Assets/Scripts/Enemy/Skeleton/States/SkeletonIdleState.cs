using UnityEngine;

public class SkeletonIdleState : SkeletonBaseState
{
    public SkeletonIdleState( SkeletonController currentContext , SkeletonStateFactory states ) 
        : base( currentContext , states ) 
    { }

    private float _actionSeconds;
    private float _maxActionSeconds;
    private bool _isMoving;

    public override void EnterState() 
    {
        _maxActionSeconds = Random.Range( 1 , 3f );
        _actionSeconds = 0;
        _isMoving = true;
    }

    public override void UpdateState()
    {
        if ( ctx.HasDetectPlayer() )
        {
            SwitchState( states.Pursuit() );
            return;
        }

        _actionSeconds += Time.deltaTime;
        if ( _actionSeconds > _maxActionSeconds )
        {
            _isMoving = !_isMoving; 
            _maxActionSeconds = Random.Range( 1 , 2f );
            _actionSeconds = 0;
        }

        if ( _isMoving )
        {
            ctx.Movement();
        }
    }
}
