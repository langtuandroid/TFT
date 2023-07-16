using System;
using UnityEngine;
using Utils;

public class PhisicalAttack : MonoBehaviour
{
    private int physicalDamage = 1;
    
    private void Update()
    {
        CheckPhisycCollisions();
    }
    
    private void CheckPhisycCollisions()
    {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(
            transform.position, 1f, LayerMask.GetMask(Constants.LAYER_INTERACTABLE));
        
        foreach (Collider2D collision in collisions)
            collision.GetComponent<IPunchanble>()?.Punch(physicalDamage);

    }
}
