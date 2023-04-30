using UnityEngine;

namespace AI
{
    public class EnemyWillOWispFollowState : FsmEnemyWillOWisp
    {
        public override void Execute(EnemyWillOWisp agent)
        {
            //Si veo o escucho al jugador mientras le persigo
            if (agent.SeePlayer() || agent.ListenPlayer())
            {
                if (!agent.SeePlayer())
                {
                    agent.ChangeState(new EnemyWillOWispPatrolState()); //Vuelvo al estado de patrulla
                } 
                else if (!agent.CheckPlayerDistance()) //Continuo siguiendole si estoy dentro del rango y no le he alcanzado
                    agent.FollowPlayer();
                else //Si le alcanzo le transporto
                {
                    agent.isTorchAction = false;
                    agent.ChangeState(new EnemyWillOWispActionState());  
                }
            } else if(!agent.SeePlayer() && !agent.ListenPlayer())  //Si dejo de verle y escucharle
            {
                agent.ResetTimer();
                agent.ChangeState(new EnemyWillOWispAlertState()); //Vuelvo al estado de alerta
            }
        }
    
    }  
}
