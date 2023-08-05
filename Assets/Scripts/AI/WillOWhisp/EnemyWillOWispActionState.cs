using UnityEngine;
namespace AI
    {
        public class EnemyWillOWispActionState : FsmEnemyWillOWisp
        {
            public override void Execute(EnemyWillOWisp agent)
            {
                //Peligro: apagar antorchas
                if (agent.SeePlayer()) //Si le veo mientras voy a apagar antorchas
                {
                    agent.ChangeState(new EnemyWillOWispFollowState());   
                }
                else if(!agent.SeePlayer())//Si no le veo
                {
                      if (agent.CheckTorchOn()) // Apago antorchas
                      {
                            agent.ChangeStatusColor("Torch");
                            agent.TorchPatrol();
                      }
                      else //Si no hay antorchas encendidas y no le veo ni le escucho
                      {
                          agent.ChangeState(new EnemyWillOWispAlertState());
                      }
                }
            }
        } 
    }
