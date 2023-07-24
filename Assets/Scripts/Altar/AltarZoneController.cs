using UnityEngine;

public class AltarZoneController : MonoBehaviour
{
    [SerializeField] private ZoneExitSideSO _zoneExitSO;
    [SerializeField] private StartRefInfoSO _startRefInfoSO;
    [SerializeField] private GameObject     _playerPrefab;

    private void Start()
    {
        PlayerInstantation();

        ServiceLocator.GetService<LevelEvents>().OnChangeZone += SaveRefPoint;
        ServiceLocator.GetService<GameStatus>().AskChangeToGamePlayState();
    }

    private void OnDestroy()
    {
        ServiceLocator.GetService<LevelEvents>().OnChangeZone -= SaveRefPoint;
    }

    private void PlayerInstantation()
    {
        int startRefIndex = GetStartRefInfoIndex( _zoneExitSO.nextStartPointRefID );
        if ( startRefIndex < 0 )
        {
            Debug.LogError( $"[CAGADA]: El punto de referencia ID: {_zoneExitSO.nextStartPointRefID} no existe en la zona" );
        }

        StartRefInfoSO.StartRefInfo startRefInfo = _startRefInfoSO.startRefInfoArray[startRefIndex];

        Debug.Log( $"From: {startRefInfo.exitZoneDescription}" );

        Vector3 position           = startRefInfo.startPosition;
        Vector2 startLookDirection = startRefInfo.PlayerStartLookDirection();
        LayerMask initialLayer     = startRefInfo.initialLayerMask;

        GameObject player = Instantiate( _playerPrefab, position, Quaternion.identity );

        player.GetComponent<Player.PlayerController>().Init( startLookDirection , initialLayer );
    }

    private int GetStartRefInfoIndex( int startPointRefID )
    {
        int arrayLength = _startRefInfoSO.startRefInfoArray.Length;
        for ( int i = 0; i < arrayLength; i++ )
        {
            int arrayStartPointRefID = _startRefInfoSO.startRefInfoArray[i].startPointRefID;
            if ( startPointRefID.Equals( arrayStartPointRefID ) )
                return i;
        }
        return -1;
    }

    private void SaveRefPoint( LevelEvents.ChangeZoneArgs changeZoneArgs )
    {
        _zoneExitSO.nextStartPointRefID = changeZoneArgs.nextStartPointRefId;
    }
}
