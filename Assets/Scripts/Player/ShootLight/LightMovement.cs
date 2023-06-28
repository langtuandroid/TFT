using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    private Vector3 _direction;
    private float lightDuration = 3.0f;
    private float lightSize = 3.0f;
    private float lightTimer = 0.0f;


    private void Awake()
    {
        //_direction = Vector3.right;
        // Set light timer and initial size
        lightTimer = lightDuration;
        transform.localScale = new Vector3(lightSize, lightSize, 1);
    }

    private void Update()
    {
        Movement();
        LiveTime();
    }


    private void Movement()
    {
        transform.position += _speed * Time.deltaTime * _direction;
    }
   
    private void LiveTime()
    {
        if (_direction == Vector3.zero) return;
        
        lightTimer -= Time.deltaTime;

        if (lightTimer >= 0f)
        {
            float normalizedTime = lightTimer / lightDuration;
            float currentSize = Mathf.Lerp(0, lightSize, normalizedTime);
            transform.localScale = new Vector3(currentSize, currentSize, 1);

        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void HandleMovement(Vector3 direction)
    {
        _direction = direction;
    }
}