using System.Collections.Generic;
using UnityEngine;

public class AltarController : MonoBehaviour
{
    [Header("Zone Related:")]
    [SerializeField] private ZoneExitSideSO _zoneExitSO;
    [SerializeField] private ZoneSaveSO     _zoneSaveSO;
    [SerializeField] private StartRefInfoSO _startRefInfoSO;
    [Header("Scene Related:")]
    [SerializeField] private GameObject     _playerPrefab;
    [SerializeField] private List<ActivableSceneObject> _activableObjectList;

    private void Start()
    {
        LoadZoneInteractableData();
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
}
