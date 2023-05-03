public class EnemySlimePatrolState : FsmEnemySlime
{
    public override void Execute(EnemySlime agent)
    {
        if (agent.ObstacleAware())
            agent.ChangeDirectionAndPatrol();
        else 
            agent.Patrol();

        if (agent.CanSeePlayer())
            agent.ChangeState(new EnemySlimeFollowPlayer());
    }
}
