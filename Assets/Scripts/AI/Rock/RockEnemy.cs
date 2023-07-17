using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using Utils;

public class RockEnemy : MonoBehaviour
{
    private IPickable _pickable;
    private PickUpItem _pickUp;
    private PlayerStatus _playerStatus;
    
    private void Awake()
    {
        _pickable = GetComponent<IPickable>();
    }

    private void Update()
    {
        if (_pickUp == null || _playerStatus == null) return;
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
        yield return new WaitForSeconds(1f);
        
        AddPlayerDamage();
        
    }

    private void AddPlayerDamage()
    {
        _playerStatus.TakeDamage(1);
        _pickable.ThrowIt(Vector2.down);
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
