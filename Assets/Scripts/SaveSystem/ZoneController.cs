using UnityEngine;

public class ZoneController : MonoBehaviour
{
    [Header("Zone Related:")]
    [SerializeField] private ZoneExitSideSO _zoneExitSO;
    [SerializeField] private ZoneSaveSO     _zoneSaveSO;
    [SerializeField] private StartRefInfoSO _startRefInfoSO;
    [Header("Scene Related:")]
    [SerializeField] private GameObject     _playerPrefab;
    [SerializeField] private Cinemachine.CinemachineVirtualCamera _camera;

    private void Start()
    {
        PlayerInstantation();
    }

    private void LoadZoneInteractableData()
    {

    }

    private void PlayerInstantation()
    {
        int startRefIndex = GetStartRefInfoIndex( _zoneExitSO.nextStartPointRefID );
        if ( startRefIndex < 0 )
        {
            Debug.LogError( $"[CAGADA]: El punto de referencia ID: {_zoneExitSO.nextStartPointRefID} no existe en la zona" );
        }

        StartRefInfoSO.StartRefInfo startRefInfo = _startRefInfoSO.startRefInfoArray[startRefIndex];

        Vector3 position           = startRefInfo.startPosition;
        Vector2 startLookDirection = startRefInfo.PlayerStartLookDirection();
        LayerMask initialLayer     = startRefInfo.initialLayerMask;

        GameObject player = Instantiate( _playerPrefab, position, Quaternion.identity );

        player.GetComponent<Player.PlayerController>().Init( startLookDirection, initialLayer );
        _camera.Follow = player.transform;
    }

    private int GetStartRefInfoIndex( int startPointRefID )
    {
        foreach ( var item in _startRefInfoSO.startRefInfoArray )
            if ( startPointRefID == item.startPointRefID )
                return item.startPointRefID;
        return -1;
    }

    private void SaveZoneInteractableData()
    {

    }
}
