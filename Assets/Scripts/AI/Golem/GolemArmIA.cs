using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using Unity.Burst.Intrinsics;
using UnityEngine;
using Utils;

public class GolemArmIA : MonoBehaviour
{
    public float speed;
    public float maxDistanceX;
    public float maxDistanceY;
    public float armRadious;
    public bool leftArm;
    public float armResistence;
    
    
    private Vector3 originalPosition;
    private bool movingRight;
    private Rigidbody2D _rb;
    private GameObject _player;
    
    
    private float originalX;
    private bool isAttacking = false;
    private bool isPuchAttacking = false;
    public float attackForce;
    private int armMovementCount;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        originalX = transform.position.x;
        armMovementCount = 0;
        originalPosition = transform.position;
        movingRight = leftArm ? true : false;
    }

    private void Update()
    {
      if(CheckPlayer()) Attack();
    }

    private bool CheckPlayer()
    {
        Collider2D results = Physics2D.OverlapCircle(transform.position, armRadious, LayerMask.GetMask(Constants.TAG_PLAYER));

        if (results != null)
        {
            if (results.CompareTag(Constants.TAG_PLAYER))
            {
                _player = results.gameObject;
                return true;
            }
        }

        return false;
    }

    private void Attack()
    {
        if (!isAttacking && !isPuchAttacking)
        {
            StartCoroutine(PerformSweep());
        }
    }
    
    public void AttackPunch()
    {
        StartCoroutine(PerformPuchAttack());
    }

    private IEnumerator PerformPuchAttack()
    {
        if (_player == null) yield return null;

        isPuchAttacking = true;
        
        yield return new WaitForSeconds(1f);
        
        Vector2 direction = (_player.transform.position - transform.position).normalized;
        
        _rb.AddForce(direction * attackForce, ForceMode2D.Impulse);
        
        yield return new WaitForSeconds(1f);
        
        isPuchAttacking = false;
        
        ResetSweepAttack();
        
    }

    private IEnumerator PerformSweep()
    {
        isAttacking = true;
            
        yield return new WaitForSeconds(1f);
        
        _rb.AddForce(leftArm ? Vector2.right : Vector2.left * attackForce, ForceMode2D.Impulse);
        
        yield return new WaitForSeconds(1f);
        
        ResetSweepAttack();
    }

    private IEnumerator PerformSweepOld()
    {
        armMovementCount++;

        isAttacking = true;
        float targetX = movingRight ? originalX + maxDistanceX : originalX - maxDistanceX;

        while (Mathf.Abs(transform.position.x - targetX) > armResistence)
        {
            float direction = movingRight ? 1f : -1f;

            Vector3 newArmPosition = new Vector3(transform.position.x, transform.position.y - maxDistanceY, transform.position.z);
            _rb.MovePosition(newArmPosition + direction * speed * Time.deltaTime * Vector3.right);

            yield return null;
        }

        _rb.MovePosition(originalPosition);

        movingRight = !movingRight;

        if (armMovementCount < 2)
            StartCoroutine(PerformSweep());
        else
            ResetSweepAttack();
    }


    private void ResetSweepAttack()
    {
        armMovementCount = 0;
        isAttacking = false;
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, armRadious);
    }
}
