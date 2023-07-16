using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using Utils;

public class RockEnemy : MonoBehaviour
{
    private IPickable _pickable;
    private PlayerController _playerController;
    private PlayerStatus _playerStatus;

    private void Awake()
    {
        _pickable = GetComponent<IPickable>();
    }

    private void Update()
    {
        if (_playerController == null || _playerStatus == null) return;
        CheckPlayer();
    }

    private void CheckPlayer()
    {
        if (_playerController.Pickable.HasItem)
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
            _playerController = col.gameObject.GetComponent<PlayerController>();
            _playerStatus = col.gameObject.GetComponent<PlayerStatus>();
        }
    }
}
