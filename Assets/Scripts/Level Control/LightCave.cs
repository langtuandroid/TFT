using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightCave : MonoBehaviour
{
    private Light2D _globalLight;
    
    [SerializeField]
    private float lightIncrement = 0.025f;

    private void Awake()
    {
        _globalLight = GetComponent<Light2D>();
    }

    public void IncreaseLightIntensity()
    {
        _globalLight.intensity += lightIncrement;
    }
}
