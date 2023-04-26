using UnityEngine;

public class EnemySlimePatrolState : FsmEnemySlime
{
    public override void Execute(EnemySlime agent)
    {
        if(!agent.CanSeePlayer()) 
            agent.Patrol();
        else
        {
            if (!agent.CanSeePlayer() && !agent.ObstacleAware()) //Si no veo al jugador y no hay obstaculo
                agent.Patrol();
            else if (agent.CanSeePlayer() && !agent.ObstacleAware()) //Veo jugador y no hay obstaculo
                agent.ChangeState(new EnemySlimeFollowPlayer());
        }
    }
}


