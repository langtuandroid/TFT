using UnityEngine;

[DefaultExecutionOrder(-10)]
public class GameSODataInstance : MonoBehaviour
{
    public static GameSODataInstance Istance { get => instance; }
    private static GameSODataInstance instance;

    [SerializeField] private PlayerStatusSaveSO _playerStatusSaveSO;
    [SerializeField] private ZoneExitSideSO     _zoneExitSideSO;
    [SerializeField] private GameZoneSavesSO    _gameZoneSavesSO;

    private void Awake()
    {
        if ( Istance == null )
        {
            instance = this;
            DontDestroyOnLoad( gameObject );
        }
        else
            Destroy( gameObject );
    }
}
