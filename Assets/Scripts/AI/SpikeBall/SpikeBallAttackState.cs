using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBallAttackState : FsmSpikeBall
{
    public override void Execute(SpikeBall agent)
    {
        agent.transform.Translate(agent.AttackVelocity * Time.deltaTime * Vector2.right);

        if (CollisionDetect(agent)) 
        {
            agent.ActualState = new SpikeBallReturnState();
        }
    }
    
    bool CollisionDetect(SpikeBall agent)
    {
        return false;
    }
}
