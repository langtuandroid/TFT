using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBallReturnState : FsmSpikeBall
{
    public override void Execute(SpikeBall agent)
    {
        Vector2 direction = (agent.InitialPosition - (Vector2)agent.transform.position).normalized;
        agent.transform.Translate(direction * agent.ReturnVelocity * Time.deltaTime);

        if (IsInitialPosition(agent)) 
        {
            agent.ActualState = new SpikeBallAlertState();
        }
    }
    
    bool IsInitialPosition(SpikeBall agent)
    {
        //float distanceToInitial = Vector2.Distance(agent.transform.position, agent.InitialPosition);
        //return distanceToInitial < agent.InitialPosition;
        return false;
    }
}
