// ************ @autor: Álvaro Repiso Romero *************
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TriggerChangeZone : MonoBehaviour
{
    [SerializeField][Range( 0, 15 )] private int _nextStartPointRefID;
    [SerializeField] private SceneName _nextScene;
    [SerializeField] private Color _fadeOutColor;
    [SerializeField] private MusicZoneParameter _musicParamName;
    [SerializeField] private MusicName _musicName;

    public UnityEvent OnZoneChanged;
    
    private float _fadeOutSeconds = 1f;

    private void OnTriggerEnter2D( Collider2D collision )
    {
        //ServiceLocator.GetService<IAudioSpeaker>().ChangeZoneParamater( _musicParamName , true );
        ServiceLocator.GetService<IAudioSpeaker>().ChangeMusic( _musicName );

        ServiceLocator.GetService<LevelEvents>().ChangeZone(
            new LevelEvents.ChangeZoneArgs
            {
                nextStartPointRefId = _nextStartPointRefID,
                fadeColor           = _fadeOutColor ,
                fadeDurationSeconds = _fadeOutSeconds
            } );

        OnZoneChanged?.Invoke();

        StartCoroutine( FadeOut() );
    }

    private IEnumerator FadeOut()
    {
        WaitForSeconds waitTime = new WaitForSeconds( _fadeOutSeconds );
        yield return waitTime;
        ServiceLocator.GetService<SceneLoader>().InstaLoad( _nextScene.ToString() );
    }
}
