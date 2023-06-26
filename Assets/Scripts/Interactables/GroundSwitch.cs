using UnityEngine;
using UnityEngine.Events;

public class GroundSwitch : MonoBehaviour
{
    [SerializeField] private bool _isHoldButton;

    public UnityEvent OnSwitchPush;
    public UnityEvent OnSwitchOff;

    private void OnTriggerEnter2D( Collider2D collision )
    {
        OnSwitchPush?.Invoke();
    }

    private void OnTriggerExit2D( Collider2D collision )
    {
        if ( _isHoldButton )
            OnSwitchOff?.Invoke();
    }
}
