using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using Utils;

public class RockEnemy : MonoBehaviour
{
    private PickUpItem _pickUp;
    
    private PlayerStatus _playerStatus;
    private Animator _animator;
    private bool _enemyCoolDown;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_pickUp == null || _playerStatus == null || _enemyCoolDown) return;
        CheckPlayer();
    }

    private void CheckPlayer()
    {
        if (_pickUp.HasItem)
        {
            StartCoroutine(nameof(AlertPhase));
        }
        else
        {
            StopCoroutine(nameof(AlertPhase));
        }
    }

    private IEnumerator AlertPhase()
    {
        _enemyCoolDown = true;
        
        _animator.SetBool("grumpy", true);

        yield return new WaitForSeconds(2f);

        if (_pickUp != null)
            _pickUp.EnemyRockThrow(Vector2.down);
        
        yield return new WaitForSeconds(0.5f);

        if (_pickUp.HasItem)
        {
            Debug.Log("Entro");
            _playerStatus.TakeDamage(1);
        }
            
        
        yield return new WaitForSeconds(1f);
        
        _animator.SetBool("grumpy", false);
        
        yield return new WaitForSeconds(1f);

        _enemyCoolDown = false;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag(Constants.TAG_PLAYER))
        {
            _pickUp = col.gameObject.GetComponent<PlayerController>().Pickable;
            _playerStatus = col.gameObject.GetComponent<PlayerStatus>();
        }
    }
}
