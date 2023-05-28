using System;
using System.Collections;
using UnityEngine;

public class MonoTimer : MonoBehaviour
{
    private Action OnDestroyObject;

    public void StartTimer( Action onTimerUp , float durationSeconds , Action onDestroyMonoTimer )
    {
        OnDestroyObject = onDestroyMonoTimer;
        StartCoroutine( Timer( onTimerUp , durationSeconds ) );
    }

    private IEnumerator Timer( Action onTimerUp , float durationSeconds )
    {
        WaitForSeconds wait = new WaitForSeconds( durationSeconds );
        yield return wait;
        onTimerUp?.Invoke();
    }

    private void OnDestroy()
    {
        OnDestroyObject?.Invoke();
    }
}
