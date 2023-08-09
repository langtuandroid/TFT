using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

public class GolemIA : MonoBehaviour
{
    [SerializeField] private GolemArmIA _leftHandIA;
    [SerializeField] private GolemArmIA _rightHandIA;
    public Camera mainCamera;
    public float PlayerDetectionRadious;
    public float speed;
    public float TotalSecondsChasingPlayer;
    private float _totalSecondsChasingPlayerTmp;

    private GolemHealth _health;
    private Animator _anim;
    private GameObject _player;
    private Rigidbody2D _rb;

    private Vector2 initialPosition;
    private float _timeChasingPlayer;

    private enum EnemyState
    {
        Idle,
        Chasing,
    }

    private EnemyState currentState = EnemyState.Idle;

    private void Awake()
    {
        _health = GetComponent<GolemHealth>();
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _health.OnDamage += DamageAttack;
        _totalSecondsChasingPlayerTmp = TotalSecondsChasingPlayer;
        _anim.Play("Idle");
        initialPosition = transform.position;
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                if (CheckPlayer()) 
                    currentState = EnemyState.Chasing; 
                break;

            case EnemyState.Chasing:
                if (_timeChasingPlayer > TotalSecondsChasingPlayer)
                    ReturnToInitialPosition();
                else
                    GoToPlayer();
                    break;
        }
    }

    private bool CheckPlayer()
    {
        Collider2D results = Physics2D.OverlapCircle(transform.position, PlayerDetectionRadious, LayerMask.GetMask(Constants.TAG_PLAYER));

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

    private void DamageAttack()
    {
        _leftHandIA?.AttackPunch();
        _rightHandIA?.AttackPunch();
    }

    private void GoToPlayer()
    {
        _anim.Play("walk");

        Vector2 direction = (_player.transform.position - transform.position).normalized;
        Vector2 velocity = direction * speed;

        transform.Translate(velocity * Time.deltaTime);

        _timeChasingPlayer += Time.deltaTime;

        currentState = EnemyState.Chasing; 
    }

    private void ReturnToInitialPosition()
    {
        _anim.Play("walk");

        Vector2 direction = (initialPosition - (Vector2)transform.position).normalized;
        Vector2 velocity = direction * speed;

        transform.Translate(velocity * Time.deltaTime);

        if (Vector2.Distance(transform.position, initialPosition) < 0.1f)
        {
            _anim.Play("Idle");
            ResetChasingTime();
        }
    }

    private void ResetChasingTime()
    {
        _timeChasingPlayer = 0f;
        currentState = EnemyState.Idle; 
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, PlayerDetectionRadious);
    }
}
