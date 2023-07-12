using UnityEngine;

namespace AI
{
  public class EnemyWillOWispPatrolState : FsmEnemyWillOWisp
{
    public override void Execute(EnemyWillOWisp agent)
    {
        //Escucho al jugador
        if (agent.ListenPlayer()) 
        {
            agent.ChangeState(new EnemyWillOWispAlertState());

        }
        //Si detecto alguna antorcha encendida y no veo al jugador
        else if (!agent.ListenPlayer() && agent.CheckTorchOn())
        {
            agent.ChangeState(new EnemyWillOWispActionState());
        }
        //***Movimiento de patrulla***
        else if (!agent.ListenPlayer() && !agent.CheckTorchOn())
        {
            agent.ChangeStatusColor("Patrol");
            
            if (Vector3.Distance(agent.transform.position, agent.ActualWayPoint().position) < 1.1f)
            {
                agent.GetNextWayPoint();
            }
            else
            {
                agent.Patrol();
            }
        } 
    }
}  
}
