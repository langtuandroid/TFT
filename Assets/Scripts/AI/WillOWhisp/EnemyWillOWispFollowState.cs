using UnityEngine;

namespace AI
{
    public class EnemyWillOWispFollowState : FsmEnemyWillOWisp
    {
        public override void Execute(EnemyWillOWisp agent)
        {
            agent.ChangeStatusColor("Danger");
           
                if (!agent.SeePlayer()) //Si dejo de verle
                {
                    if(!agent.CanSee)
                        agent.ChangeState(new EnemyWillOWispAlertState()); //Vuelvo al estado de alerta
                } 
                else if (agent.SeePlayer()) // Persigo al jugador
                {
                    if (agent.CheckPlayerDistance()) // Si le alcanzo le saco fuera
                            agent.Reset();
                    else // Si no le persigo
                            agent.FollowPlayer();
                }
        }
    
    }  
}
