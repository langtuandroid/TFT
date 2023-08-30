using UnityEngine;

namespace AI
{
    public class EnemyWillOWispAlertState : FsmEnemyWillOWisp
    {
        public override void Execute(EnemyWillOWisp agent)
        {
            if (agent.SeePlayer())// Veo al jugador
                {
                    if (agent.ObstacleDetection()) // Si persiguiendole me encuentro un obstaculo me paro y vuelvo a patrullar
                    {
                        agent.ChangeStatusColor("Alert");
                        
                        if (agent.SecondsListening > 0) //Espero para patrullar 
                        {
                            agent.ChangeNavMeshAgentSpeed(0f);
                            agent.SecondsListening -= Time.deltaTime;
                        }
                        else
                        {
                            agent.ResetListenTimer();
                            agent.ChangeNavMeshAgentSpeed(agent.Speed);
                            agent.ChangeState(new EnemyWillOWispPatrolState()); 
                        }
                    }
                    else // Le persigo
                    {
                        agent.ChangeNavMeshAgentSpeed(agent.Speed);
                        agent.ChangeState(new EnemyWillOWispFollowState());
                    }

                }
                else
                {
                    agent.ChangeStatusColor("Alert");
                    
                    if (agent.SecondsListening > 0) //Espero para patrullar 
                    {
                        agent.ChangeNavMeshAgentSpeed(0f);
                        agent.SecondsListening -= Time.deltaTime;
                    }
                    else
                    {
                        agent.ResetListenTimer();
                        agent.ChangeNavMeshAgentSpeed(agent.Speed);
                        agent.ChangeState(new EnemyWillOWispPatrolState()); 
                    }
                }
            }
    }
}
