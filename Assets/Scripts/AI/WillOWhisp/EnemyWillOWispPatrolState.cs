using UnityEngine;

namespace AI
{
  public class EnemyWillOWispPatrolState : FsmEnemyWillOWisp
{
    public override void Execute(EnemyWillOWisp agent)
    {
        agent.ChangeStatusColor("Patrol");
        //Escucho al jugador y no le veo
        if (agent.ListenPlayer() && !agent.SeePlayer()) 
        {
            agent.ResetTimer();
            agent.ChangeState(new EnemyWillOWispAlertState()); //Me pongo en alerta 
        }
    
        //Veo al jugador
        else if (agent.SeePlayer()) 
        {
            agent.ChangeState(new EnemyWillOWispFollowState()); //Persigo al jugador si lo veo 
        }
        
        //Si detecto alguna antorcha encendida
        else if (agent.CheckTorchOn())
        {
            agent.IsTorchAction = true;
            agent.ChangeState(new EnemyWillOWispActionState());
        }
        //Movimiento de patrulla
        else if (!agent.ListenPlayer() && !agent.SeePlayer())
        {
            if (Vector3.Distance(agent.transform.position, agent.ActualWayPoint().position) < 1.1f)
            {
                agent.UpdatePatrolWayPoint(agent.GetNextWayPoint());
            }
        } 
    }
}  
}
