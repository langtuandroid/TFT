using UnityEngine;
using UnityEngine.Events;

public class GroundTrigger : MonoBehaviour
{
    public UnityEvent OnTriggerEnter;

    private void OnTriggerEnter2D( Collider2D collision )
    {
        OnTriggerEnter?.Invoke();
    }
}
