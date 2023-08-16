using System;
using Player;
using UnityEngine;
using Utils;

public class PhisicalAttack : MonoBehaviour
{
    private int physicalDamage = 1;
    private bool _canCheckPhysicalCollisions;
    private AnimatorBrain _animatorBrain;

    private void Update()
    {
        if(_canCheckPhysicalCollisions) CheckPhisycCollisions();
    }

    public void Attack(AnimatorBrain animatorBrain, Boolean physicAction)
    {
        if (!physicAction) return;

        _animatorBrain = animatorBrain;
        _animatorBrain.SetPhysicalAttack();

        _canCheckPhysicalCollisions = true;
    }

    public void ResetPhysicAttackCollisions() => _canCheckPhysicalCollisions = false;

    private void CheckPhisycCollisions()
    {
        if (_animatorBrain.HasCurrentAnimationEnded())
            ResetPhysicAttackCollisions();
        
        Collider2D[] collisions = Physics2D.OverlapCircleAll(
            transform.position, 1f, LayerMask.GetMask(Constants.LAYER_INTERACTABLE));
        
        foreach (Collider2D collision in collisions)
            collision.GetComponent<IPunchable>()?.Punch(physicalDamage);
    }
}
