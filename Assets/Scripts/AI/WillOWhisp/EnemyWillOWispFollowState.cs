using UnityEngine;

namespace AI
{
    public class EnemyWillOWispFollowState : FsmEnemyWillOWisp
    {
        public override void Execute(EnemyWillOWisp agent)
        {
            agent.ChangeStatusColor("Danger");
            if (agent.SeePlayer() || agent.ListenPlayer())
            {
                if (!agent.SeePlayer())
                {
                    agent.ChangeState(new EnemyWillOWispAlertState()); //Vuelvo al estado de alerta
                } 
                else if (agent.SeePlayer())
                {
                    if (agent.TeleporPlayer)
                    {
                        agent.IsTorchAction = false;
                        agent.ChangeState(new EnemyWillOWispActionState());     
                    }
                    else
                    {
                        //Continuo siguiendole si estoy dentro del rango y no le he alcanzado
                        agent.FollowPlayer();
                    }
                }
            } else if(!agent.SeePlayer() && !agent.ListenPlayer())  //Si dejo de verle o sale del ratio teleport
            {
                agent.ResetTimer();
                agent.ChangeState(new EnemyWillOWispAlertState()); //Vuelvo al estado de alerta
            }
        }
    
    }  
}
