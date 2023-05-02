using UnityEngine;

public class EnemySlimePatrolState : FsmEnemySlime
{
    public override void Execute(EnemySlime agent)
    {
        if (!agent.CanSeePlayer())
        {
            if (agent.CanChangePatrolDirection())
            {
                agent.Patrol();  
            }
        }
        else
        {
            if (!agent.CanSeePlayer() && !agent.ObstacleAware())//Si no veo al jugador y no hay obstaculo
            {
                if (agent.CanChangePatrolDirection())
                {
                    agent.Patrol();  
                }
            } 
            else if (agent.CanSeePlayer() && !agent.ObstacleAware()) //Veo jugador y no hay obstaculo
                agent.ChangeState(new EnemySlimeFollowPlayer());
        }
    }
}


