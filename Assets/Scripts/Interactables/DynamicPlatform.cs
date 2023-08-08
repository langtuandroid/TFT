using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicPlatform : MonoBehaviour
{
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;
    [SerializeField] private float _speed;

    private bool _canMove = true;
    private Transform _target;

    private void Start()
    {
        _target = _pointA;
    }

    private void Update()
    {
        if (_canMove)
        {
            Move();
        }
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
        transform.position = Vector3.MoveTowards(transform.position, _target.position, step);
    }

    public void ActivatePlatform() => _canMove = true;
}