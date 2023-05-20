using UnityEngine;

public class TriggerChangeZone : MonoBehaviour
{
    [SerializeField][Range( 0, 10 )] private int _nextStartPointRefID;
    [SerializeField] private ZoneExitSideSO _zoneExitSO;

    private void OnTriggerEnter2D( Collider2D collision )
    {
        if ( collision.CompareTag( "Player" ) )
            ServiceLocator.GetService<LevelEvents>().ChangeZone();
    }
}
