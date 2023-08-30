// ************ @autor: Álvaro Repiso Romero *************
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TriggerChangeZone : MonoBehaviour
{
    [SerializeField] private ChangeSceneInfoSO _changeSceneInfoSO;
    [Space(20)] public UnityEvent OnZoneChanged;

    private void OnTriggerEnter2D( Collider2D collision )
    {
        ServiceLocator.GetService<GameStatus>().AskChangeToInactiveState();
        ServiceLocator.GetService<IAudioSpeaker>().ChangeMusic( _changeSceneInfoSO.MusicName );

        ServiceLocator.GetService<LevelEvents>().ChangeZone(
            new LevelEvents.ChangeZoneArgs
            {
                nextStartPointRefId = _changeSceneInfoSO.NextStartPointRefID ,
                fadeColor           = _changeSceneInfoSO.FadeOutColor ,
                fadeDurationSeconds = _changeSceneInfoSO.FadeOutSeconds
            } );

        OnZoneChanged?.Invoke();

        StartCoroutine( FadeOut() );
    }

    private IEnumerator FadeOut()
    {
        var waitTime = new WaitForSeconds( _changeSceneInfoSO.FadeOutSeconds );
        yield return waitTime;
        ServiceLocator.GetService<SceneLoader>().InstaLoad( _changeSceneInfoSO.NextScene );
    }
}
