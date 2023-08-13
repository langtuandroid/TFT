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
                        
                        Debug.Log("Segundos restantes para volver a patrulla: " + agent.SecondsListening);
                    
                        if (agent.SecondsListening > 0) //Espero para patrullar 
                        {
                            agent.ChangeNavMeshAgentSpeed(0f);
                            agent.SecondsListening -= Time.deltaTime;
                        }
                        else
                        {
                            agent.ResetListenTimer();
                            agent.ChangeNavMeshAgentSpeed(2.5f);
                            agent.ChangeState(new EnemyWillOWispPatrolState()); 
                        }
                    }
                    else // Le persigo
                    {
                        agent.ChangeNavMeshAgentSpeed(2.5f);
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
                        agent.ChangeNavMeshAgentSpeed(2.5f);
                        agent.ChangeState(new EnemyWillOWispPatrolState()); 
                    }
                }
            }
    }
}
