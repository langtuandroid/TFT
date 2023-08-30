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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(agent.transform.position, agent.PatrolDetectionRadius);
        
        if (colliders == null) return false;
        
        foreach (Collider2D collider in colliders)
        {
            if (collider == null) return false;
            if (collider != null)
            {
                Debug.Log(collider.gameObject.name);
                return true; 
            }
        }

        return false; 
    }
}
