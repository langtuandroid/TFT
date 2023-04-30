using UnityEngine;

namespace AI
{
    public class EnemyWillOWispAlertState : FsmEnemyWillOWisp
    {
        public override void Execute(EnemyWillOWisp agent)
        {
            agent._navMeshAgent.speed = 3.5f;
            if (agent.SeePlayer() && agent.ListenPlayer()) // Si veo y escucho al jugador
            {
                agent.ChangeState(new EnemyWillOWispFollowState()); //Persigo al jugador si lo veo 
            } else if (agent.ListenPlayer() && !agent.SeePlayer()) // Si escucho al jugador y no lo veo
            {
                if (agent.CheckTorchOn()) //Voy a por las antorchas si hay alguna encendida
                {
                    agent.IsTorchAction = true;
                    agent.ChangeState(new EnemyWillOWispActionState());
                }
                else // Espero y entonces voy a patrulla
                {
                    if (agent.SecondsListening > 0) //Mientras espero para patrullar 
                    {
                        agent._navMeshAgent.speed = 0;
                        agent.SecondsListening -= Time.deltaTime;
                    }
                    else
                    {
                        agent.CanListen = false;
                        agent.ResetTimer();
                        agent.ChangeState(new EnemyWillOWispPatrolState()); //Vuelvo a patrullar tras esperar 
                    }
                   
                }
            } else if (!agent.SeePlayer() && !agent.ListenPlayer()) //Si no veo ni escucho al jugador
            {
                agent.ChangeState(new EnemyWillOWispPatrolState()); //Vuelvo a patrullar tras esperar 
            } 
        }
    }
}
