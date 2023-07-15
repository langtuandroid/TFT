// ************ @autor: �lvaro Repiso Romero *************
using Player;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private List<ActivableSceneObject> _activableObjectList;
    [SerializeField] private Collider2D[] _cameraConfinerColliderArray;

    private PlayerController _playerController;

    private void Start()
    {
        LoadZoneInteractableData();
        PlayerInstantation();

        ServiceLocator.GetService<LevelEvents>().OnChangeZone    += SaveZoneData;
        ServiceLocator.GetService<LevelEvents>().OnZoneCompleted += ZoneComplete;
        ServiceLocator.GetService<LifeEvents>().OnFallDown       += MovePlayerToEnter;
    }

    private void OnDestroy()
    {
        ServiceLocator.GetService<LevelEvents>().OnChangeZone    -= SaveZoneData;
        ServiceLocator.GetService<LevelEvents>().OnZoneCompleted -= ZoneComplete;
        ServiceLocator.GetService<LifeEvents>().OnFallDown       -= MovePlayerToEnter;
    }

    private void LoadZoneInteractableData()
    {
        if ( _zoneSaveSO.zoneSave.IsCompleted )
        {
            Debug.Log( "Zone completed, TODO: Create logic" );
        }

        List<bool> activatedObjects = _zoneSaveSO.zoneSave.IsActivatedList;
        for ( int i = 0; i < activatedObjects.Count; i++ )
            if ( activatedObjects[i] )
                _activableObjectList[i].TriggerActivation();
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

        _playerController = player.GetComponent<PlayerController>();
        _playerController.Init( startLookDirection, initialLayer );
        _camera.Follow = player.transform;

        Cinemachine.CinemachineConfiner2D cinemaConfiner = _camera.GetComponent<Cinemachine.CinemachineConfiner2D>();
        cinemaConfiner.InvalidateCache();
        cinemaConfiner.m_BoundingShape2D = _cameraConfinerColliderArray[startRefInfo.confinerColliderIndex];
        _camera.GetComponent<Cinemachine.CinemachineConfiner2D>().InvalidateCache();
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

    public void ZoneComplete()
    {
        _zoneSaveSO.zoneSave.IsCompleted = true;
    }

    private void SaveZoneData( LevelEvents.ChangeZoneArgs changeZoneArgs )
    {
        _zoneExitSO.nextStartPointRefID = changeZoneArgs.nextStartPointRefId;

        _zoneSaveSO.zoneSave.IsActivatedList = new List<bool>();
        for ( int i = 0; i < _activableObjectList.Count; i++ )
            _zoneSaveSO.zoneSave.IsActivatedList.Add( _activableObjectList[i].HasBeenActivated() );
    }


    private void MovePlayerToEnter() { StartCoroutine( MovePlayer() ); }

    private IEnumerator MovePlayer()
    {
        int startRefIndex = GetStartRefInfoIndex( _zoneExitSO.nextStartPointRefID );
        StartRefInfoSO.StartRefInfo startRefInfo = _startRefInfoSO.startRefInfoArray[startRefIndex];

        Vector3 position           = startRefInfo.startPosition;
        Vector2 startLookDirection = startRefInfo.PlayerStartLookDirection();
        LayerMask initialLayer     = startRefInfo.initialLayerMask;

        while ( _playerController.transform.position != position )
        {
            Vector2 direction = position - _playerController.transform.position;
            _playerController.MoveExternally( direction.normalized );
            yield return null;
        }

        _playerController.FallRecovery( startLookDirection, initialLayer );
    }
}
