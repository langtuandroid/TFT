using UnityEngine;
using UnityEngine.Events;

public class GroundSwitch : MonoBehaviour
{
    [SerializeField] private bool _isHoldButton;
    [SerializeField] private bool _isOnceUseButton;

    [Space(10)]
    public UnityEvent OnSwitchPush;
    public UnityEvent OnSwitchOff;

    private bool _hasBeenUsed = false;

    private void OnTriggerEnter2D( Collider2D collision )
    {
        if ( _isOnceUseButton )
        {
            if ( !_hasBeenUsed )
            {
                _hasBeenUsed = true;
                OnSwitchPush?.Invoke();
            }
        }
        else
        {
            OnSwitchPush?.Invoke();
        }
    }

    private void OnTriggerExit2D( Collider2D collision )
    {
        if ( _isHoldButton )
            OnSwitchOff?.Invoke();
    }
}
