using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using Utils;

public class DynamicPlatform : MonoBehaviour
{
    [SerializeField] private List<Transform> _pointsList;
    
    [SerializeField] private float _speed;
    public float PlayerDetectionRadious;

    public bool CanMove = false;

    private Transform _target;
    private Transform _playerOnPlatform;
    private PlayerController _playerController;

    private int pointIndex = 0;
    private bool _moveToEnd = true;
    
    private void Update()
    {
        if (CanMove)
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
        float step = _speed * Time.deltaTime;
        Vector3 newPosition = Vector3.MoveTowards(transform.position, _pointsList[pointIndex].position, step);

        // Player Position
        if (_playerOnPlatform != null)
        {
            Vector3 deltaMovement = newPosition - transform.position;
            _playerOnPlatform.position += deltaMovement;
        }

        if (Vector3.Distance(transform.position, _pointsList[pointIndex].position) < 0.1f)
        {
            if (_moveToEnd)
            {
                if (pointIndex == _pointsList.Count - 1)
                {
                    _moveToEnd = false;
                    pointIndex--;
                }
                else
                {
                    pointIndex++;
                }
            }
            else
            {
                if (pointIndex == 0)
                {
                    _moveToEnd = true;
                    pointIndex++;
                }
                else
                {
                    pointIndex--;
                }
            }
        }

        transform.position = newPosition;
    }

    public void ActivatePlatform()
    {
        CanMove = true;
        _playerOnPlatform = null;
    }

    public void DeactivatePlatform()
    {
        CanMove = false;
        _playerOnPlatform = null;
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, PlayerDetectionRadious);
    }
}
