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
               if(agent.CheckPlayerDistance())
                agent.Reset();
            }
            //Peligro: apagar antorchas
            else
            {
                if (agent.SeePlayer())
                {
                    agent.IsTorchAction = false;
                    agent.ChangeState(new EnemyWillOWispFollowState());
                }
                else
                {
                    if (agent.CheckTorchOn()) //Si apagando alguna antorcha veo o escucho al jugador voy a por el
                    {
                        if (agent.SeePlayer() || agent.ListenPlayer())
                        {
                            agent.IsTorchAction = false;
                            agent.ChangeState(new EnemyWillOWispAlertState());
                        }
                        else if (!agent.SeePlayer() && !agent.ListenPlayer())
                        {
                            agent.Init();
                        }
                        else
                        {
                            agent.TorchPatrol();
                        }
                    }
                    else
                    {
                        agent.Init();
                    }
                }
            }
        }
    } 
}
