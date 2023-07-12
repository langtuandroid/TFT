using UnityEngine;

namespace AI
{
    public class EnemyWillOWispAlertState : FsmEnemyWillOWisp
    {
        public override void Execute(EnemyWillOWisp agent)
        { 
            if (agent.ListenPlayer()) // Si escucho al jugador
            {
                if (agent.SeePlayer())
                {
                    if (agent.ObstacleDetection())
                    {
                        agent.ChangeNavMeshAgentSpeed(0f);
                    }
                    else
                    {
                        agent.ChangeNavMeshAgentSpeed(2.5f);
                        agent.ChangeState(new EnemyWillOWispFollowState());
                    }

                }
                else if (agent.CheckTorchOn()) //Voy a por las antorchas si hay alguna encendida
                {
                    agent.ChangeNavMeshAgentSpeed(3.5f);
                    agent.ChangeState(new EnemyWillOWispActionState());
                }
                else // Espero y entonces voy a patrulla
                {
                    agent.ChangeStatusColor("Alert");
                    
                    if (agent.SecondsListening > 0) //Espero para patrullar 
                    {
                        agent.ChangeNavMeshAgentSpeed(0f);
                        agent.SecondsListening -= Time.deltaTime;
                    }
                    else
                    {
                        agent.CanListen = false;
                        agent.ResetListenTimer();
                        agent.ChangeNavMeshAgentSpeed(2f);
                        //Vuelvo a patrullar tras esperar 
                    }
                }
            }
            else
                agent.ChangeState(new EnemyWillOWispPatrolState()); 
        }
    }
}
