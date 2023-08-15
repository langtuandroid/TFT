using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    private Vector3 _direction;
    private float lightDuration = 3.0f;
    private float lightSize = 3.0f;
    private float lightTimer = 0.0f;
    private Light2D _light2DVolumen;

    private void Awake()
    {
        _light2DVolumen = GetComponentInChildren<Light2D>();
        lightTimer = lightDuration;
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
            _light2DVolumen.pointLightInnerRadius -= 0.008f;
            _light2DVolumen.pointLightOuterRadius -= 0.008f;
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