using UnityEngine;

public class EnemySlimeFollowPlayer : FsmEnemySlime
{
    private bool _obstacleAware;
    
    public override void Execute(EnemySlime agent)
    {
        if (_obstacleAware) return;
        _obstacleAware = agent.ObstacleAware();
        
        if (agent.CanSeePlayer() && !_obstacleAware)
            agent.Follow();

        else if (!agent.CanSeePlayer() || !_obstacleAware)
            agent.ChangeState(new EnemySlimePatrolState());
    }
}
