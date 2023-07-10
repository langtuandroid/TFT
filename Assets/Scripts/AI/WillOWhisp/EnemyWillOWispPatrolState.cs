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
            agent.ChangeState(new EnemyWillOWispAlertState()); //Me pongo en alerta 
        }
        //Veo al jugador
        else if (agent.ListenPlayer() && agent.SeePlayer()) 
        {
            if (!agent.CanSee) // He detectado alguna colision que no es el player
            {
                agent.ChangeState(new EnemyWillOWispAlertState()); //Me pongo en alerta 
            }
            else
            {
                agent.ChangeState(new EnemyWillOWispFollowState()); //Persigo al jugador si lo veo        
            }
        }
        //Si detecto alguna antorcha encendida y no le veo ni escucho
        else if (!agent.ListenPlayer() && !agent.SeePlayer() && agent.CheckTorchOn())
        {
            agent.ChangeState(new EnemyWillOWispActionState());
        }
        //Movimiento de patrulla
        else if (!agent.ListenPlayer() && !agent.SeePlayer() && !agent.CheckTorchOn())
        {
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
