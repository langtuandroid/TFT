using UnityEngine;
using UnityEngine.Events;

public class StrikeSwitch : ActivableSceneObject, IBurnable
{
    [SerializeField] private bool _isOn;

    public UnityEvent OnSwitchOn;
    public UnityEvent OnSwitchOff;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.color = _isOn ? Color.green : Color.red;
    }

    public void Switch()
    {
        if ( _isOn ) SwitchOff();
        else         SwitchOn();
    }

    public void SwitchOn()
    {
        _isOn = true;
        OnSwitchOn?.Invoke();
        _spriteRenderer.color = Color.green;
    }
    
    public void SwitchOff()
    {
        _isOn = false;
        OnSwitchOff?.Invoke();
        _spriteRenderer.color = Color.red;
    }

    public void Burn( int damage )
    {
        Switch();
    }
}
