using UnityEngine;

namespace AI
{
    public class EnemyWillOWispFollowState : FsmEnemyWillOWisp
    {
        public override void Execute(EnemyWillOWisp agent)
        {
            if (agent.SeePlayer()) // Persigo al jugador
            {
                    if (agent.ObstacleDetection()) // Si hay algo entre fuego fatuo y player
                    {
                        agent.ChangeState(new EnemyWillOWispAlertState());
                    }
                    else
                    {
                        agent.ChangeStatusColor("Danger");
                        
                        if (agent.CheckPlayerDistance()) // Si le alcanzo le saco fuera
                            agent.Reset();
                        else // Si no le persigo
                            agent.FollowPlayer();
                    }
              
            }
            else if(!agent.SeePlayer())
            {
                    agent.ResetListenTimer();
                    agent.ChangeState(new EnemyWillOWispAlertState()); //Vuelvo al estado de alerta
            }
        }
    }  
}
