// ************ @autor: Álvaro Repiso Romero *************
using System.Collections;
using UnityEngine;

public class TriggerChangeZone : MonoBehaviour
{
    [SerializeField][Range( 0, 15 )] private int _nextStartPointRefID;
    [SerializeField] private SceneName _nextScene;
    [SerializeField] private Color _fadeOutColor;
    
    private float _fadeOutSeconds = 1f;

    private void OnTriggerEnter2D( Collider2D collision )
    {
        if ( collision.CompareTag( "Player" ) )
        {
            StartCoroutine( FadeOut() );
            Debug.Log( _nextStartPointRefID );
            ServiceLocator.GetService<LevelEvents>().ChangeZone(
                new LevelEvents.ChangeZoneArgs
                {
                    nextStartPointRefId = _nextStartPointRefID,
                    fadeColor           = _fadeOutColor ,
                    fadeDurationSeconds = _fadeOutSeconds
                } );
        }
    }

    private IEnumerator FadeOut()
    {
        WaitForSeconds waitTime = new WaitForSeconds( _fadeOutSeconds );
        yield return waitTime;
        ServiceLocator.GetService<SceneLoader>().InstaLoad( _nextScene.ToString() );
    }

    
}
