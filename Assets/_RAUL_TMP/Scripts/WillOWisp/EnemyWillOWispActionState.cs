using UnityEngine;

namespace AI
{
    public class EnemyWillOWispActionState : FsmEnemyWillOWisp
    {
        public override void Execute(EnemyWillOWisp agent)
        {
            //Si alcanzo al jugador
            if (!agent.isTorchAction)
            {
                TransitionManager.instance.CrossFade();
                agent.Reset();
            }
            //Peligro: apagar antorchas
            else
            {
                if (agent.SeePlayer())
                {
                    agent.isTorchAction = false;
                }
                else
                {
                    if (agent.torchOnList.Count > 0)
                    {
                        //agent.TorchPatrol();
                    }
                    else
                    {
                        //agent.TorchReset();
                        agent.ChangeState(new EnemyWillOWispPatrolState());
                    }
                
                    
                }
      
            }

      
        }
    } 
}
