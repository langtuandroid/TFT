// ************ @autor: �lvaro Repiso Romero *************
using System.Collections;
using UnityEngine;

public class TriggerChangeZone : MonoBehaviour
{
    [SerializeField][Range( 0, 15 )] private int _nextStartPointRefID;
    [SerializeField] private ZoneExitSideSO _zoneExitSO;
    [SerializeField] private SceneName _nextScene;

    private void OnTriggerEnter2D( Collider2D collision )
    {
        if ( collision.CompareTag( "Player" ) )
        {
            StartCoroutine( FadeOut() );
            ServiceLocator.GetService<LevelEvents>().ChangeZone();
        }
    }

    private IEnumerator FadeOut()
    {
        WaitForSeconds waitTime = new WaitForSeconds( 1f );
        yield return waitTime;
        ServiceLocator.GetService<SceneLoader>().InstaLoad( _nextScene.ToString() );
    }
}
