using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class SpikeBallAlertState : FsmSpikeBall
{
    private bool movingRight = true; 
    public float patrolSpeed = 2.0f;
    private float detectionTime = 0.3f;
    private float detectionTimeTmp = 0.3f;
    
    public override void Execute(SpikeBall agent)
    {
        if (CheckPlayerOrEnemy(agent)) // Comprobamos si el jugador o un enemigo está cerca
        {
            agent.ActualState = new SpikeBallAttackState();
        }
        else
        {
            Patrol(agent); // Si no se detecta al jugador ni a un enemigo -> patrulla
        }
    }
    
    bool CheckPlayerOrEnemy(SpikeBall agent)
    {
        Collider2D colliders = Physics2D.OverlapCircle(agent.transform.position, agent.DetectionRadius, LayerMask.GetMask(Constants.TAG_PLAYER));
        
        if (colliders == null) return false;
        
 
            if (colliders.CompareTag(Constants.TAG_PLAYER) || colliders.CompareTag(Constants.TAG_ENEMY))
            {
                return true; 
            }
        

        return false; 
    }
    
    void Patrol(SpikeBall agent)
    {
        Vector2 patrolDirection = movingRight ? Vector2.right : Vector2.left;
        agent.transform.Translate(patrolDirection * patrolSpeed * Time.deltaTime);

        // Cambiar de dirección si llega a un límite
        if (CheckPatrolLimits(agent))
        {
            movingRight = !movingRight;
        }
    }

    bool CheckPatrolLimits(SpikeBall agent)
    {
        detectionTimeTmp -= Time.deltaTime;
        if (detectionTimeTmp > 0f) return false;
        else detectionTimeTmp = detectionTime;
        
        Collider2D colliders = Physics2D.OverlapCircle(agent.transform.position, agent.PatrolDetectionRadius, LayerMask.GetMask("Bounds"));
        
        if (colliders == null) return false;
        else return true; 
    }
}
