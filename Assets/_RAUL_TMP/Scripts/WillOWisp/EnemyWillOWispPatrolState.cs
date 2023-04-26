using UnityEngine;

public class EnemyWillOWispPatrolState : FsmEnemyWillOWisp
{
    public override void Execute(EnemyWillOWisp agent)
    {
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
            agent.isTorchAction = true;
            agent.ChangeState(new EnemyWillOWispActionState());
        }
        //Movimiento de patrulla
        else if (!agent.ListenPlayer() && !agent.SeePlayer()) //TODO cambiar por una lista
        {
            if (agent.actualWayPoint == 1)
            {
                agent.UpdatePatrolWayPoint(agent.wayPoint2);
                if (Vector3.Distance(agent.transform.position, agent.wayPoint2.transform.position) < 0.1f)
                {
                    agent.actualWayPoint = 2;
                }
            } else if (agent.actualWayPoint == 2)
            {
                agent.UpdatePatrolWayPoint(agent.wayPoint3);
                if (Vector3.Distance(agent.transform.position, agent.wayPoint3.transform.position) < 0.1f)
                {
                    agent.actualWayPoint = 3;
                }
            }
            else if (agent.actualWayPoint == 3)
            {
                agent.UpdatePatrolWayPoint(agent.wayPoint4);
                if (Vector3.Distance(agent.transform.position, agent.wayPoint4.transform.position) < 0.1f)
                {
                    agent.actualWayPoint = 4;
                }
            }
            else if (agent.actualWayPoint == 4)
            {
                agent.UpdatePatrolWayPoint(agent.wayPoint1);
                if (Vector3.Distance(agent.transform.position, agent.wayPoint1.transform.position) < 0.1f)
                {
                    agent.actualWayPoint = 1;
                }
            }
        } 
    }
}