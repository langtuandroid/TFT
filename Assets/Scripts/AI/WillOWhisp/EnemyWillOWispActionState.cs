using UnityEngine;

namespace AI
{
    public class EnemyWillOWispActionState : FsmEnemyWillOWisp
    {
        public override void Execute(EnemyWillOWisp agent)
        {
            //Si alcanzo al jugador
            if (!agent.IsTorchAction)
            {
                //TransitionManager.instance.CrossFade();
                agent.Reset();
            }
            //Peligro: apagar antorchas
            else
            {
                if (agent.SeePlayer())
                {
                    agent.IsTorchAction = false;
                }
                else
                {
                    if (agent.TorchPatrol()) return;
                    else agent.ChangeState(new EnemyWillOWispAlertState());
                }
            }
        }
    } 
}
