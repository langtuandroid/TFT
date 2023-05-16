public class EnemySlimeFollowPlayer : FsmEnemySlime
{
    public override void Execute(EnemySlime agent)
    {
        if (agent.CanSeePlayer())
            agent.Follow();
        else
            agent.ChangeState(new EnemySlimePatrolState());
    }
}