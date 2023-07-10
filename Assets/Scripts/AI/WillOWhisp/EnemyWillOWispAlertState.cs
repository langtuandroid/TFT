using UnityEngine;

namespace AI
{
    public class EnemyWillOWispAlertState : FsmEnemyWillOWisp
    {
        public override void Execute(EnemyWillOWisp agent)
        {
            agent.ChangeStatusColor("Alert");
            
            if (agent.SeePlayer() && agent.ListenPlayer()) // Si veo y escucho al jugador
            {
                if (!agent.CanSee) // He detectado alguna colision que no es el player
                {
                    agent.ResetSeeSense();
                }
                else
                {
                    agent.ChangeNavMeshAgentSpeed(3.5f);
                    agent.ChangeState(new EnemyWillOWispFollowState()); //Persigo al jugador si lo veo    
                }
            } 
            else if (!agent.SeePlayer() && agent.ListenPlayer()) // Si escucho al jugador y no lo veo
            {
                if (agent.CheckTorchOn()) //Voy a por las antorchas si hay alguna encendida
                {
                    agent.ChangeNavMeshAgentSpeed(3.5f);
                    agent.ChangeState(new EnemyWillOWispActionState());
                }
                else // Espero y entonces voy a patrulla
                {
                    if (agent.SecondsListening > 0) //Espero para patrullar 
                    {
                        agent.ChangeNavMeshAgentSpeed(0f);
                        agent.SecondsListening -= Time.deltaTime;
                    }
                    else
                    {
                        agent.CanListen = false;
                        agent.ResetListenTimer();
                        agent.ChangeNavMeshAgentSpeed(3.5f);
                        agent.ChangeState(new EnemyWillOWispPatrolState()); //Vuelvo a patrullar tras esperar 
                    }
                   
                }
            } 
            else if (!agent.SeePlayer() && !agent.ListenPlayer()) //Si no veo ni escucho al jugador
            {
                agent.ChangeNavMeshAgentSpeed(3.5f);
                agent.ChangeState(new EnemyWillOWispPatrolState()); //Vuelvo a patrullar tras esperar 
            } 
        }
    }
}
