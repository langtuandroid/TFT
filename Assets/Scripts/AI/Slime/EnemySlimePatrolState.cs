public class EnemySlimePatrolState : FsmEnemySlime
{
    public override void Execute(EnemySlime agent)
    {
        if (agent.CanSeePlayer())
            agent.ChangeState(new EnemySlimeFollowPlayer());
        else 
            agent.Patrol();
    }
}
