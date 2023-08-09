using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using Utils;

public class DynamicPlatform : MonoBehaviour
{
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;
    [SerializeField] private float _speed;
    public float PlayerDetectionRadious;

    private bool _canMove = false;
   // public bool CanMove = { set => _canMove.va}
    
    private Transform _target;
    private Transform _playerOnPlatform;
    private PlayerController _playerController;
    
    private void Start()
    {
        _target = _pointA;
    }

    private void Update()
    {
        if (_canMove)
        {
            Move();
            CheckPlayer();
        }
    }
    
    private void CheckPlayer()
    {
        Collider2D results = Physics2D.OverlapCircle(transform.position, PlayerDetectionRadious, LayerMask.GetMask(Constants.TAG_PLAYER));

        if (results != null)
        {
            if (results.CompareTag(Constants.TAG_PLAYER))
            {
                if (_playerController == null) results.GetComponent<PlayerController>();
                _playerOnPlatform = results.transform;
                return;
            }
        }

        _playerOnPlatform = null;
    }
    
    private void Move()
    {
        float distanceToTarget = Vector3.Distance(transform.position, _target.position);
        
        if (distanceToTarget < 0.1f)
        {
            if (_target == _pointA)
            {
                _target = _pointB;
            }
            else
            {
                _target = _pointA;
            }
        }

        float step = _speed * Time.deltaTime;
        Vector3 newPosition = Vector3.MoveTowards(transform.position, _target.position, step);

        // Player Position
        if (_playerOnPlatform != null)
        {
            Vector3 deltaMovement = newPosition - transform.position;
            _playerOnPlatform.position += deltaMovement;
        }

        transform.position = newPosition;
    }

    public void ActivatePlatform()
    {
        _canMove = true;
        _playerOnPlatform = null;
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, PlayerDetectionRadious);
    }
}
