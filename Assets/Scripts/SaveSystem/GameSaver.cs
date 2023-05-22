// ************ @autor: Álvaro Repiso Romero *************
using UnityEngine;

public class GameSaver : MonoBehaviour, IInteractable
{
    [SerializeField][Range( 0, 3 )] private int _savePointRef;
    [SerializeField] private GameObject         _exclamationIcon;
    [SerializeField] private PlayerStatusSaveSO _playerStatusSaveSO;
    [SerializeField] private GameZoneSavesSO    _gameZoneSavesSO;

    public void Interact( Vector2 lookDirection )
    {
        SaveData();
    }

    public void ShowCanInteract( bool show )
    {
        _exclamationIcon.SetActive( show );
    }

    private void SaveData()
    {
        SaveGame saveGame = new SaveGame();
        saveGame.SaveOptions( ServiceLocator.GetService<OptionsSave>() );
        saveGame.SavePlayerGame( 1 , new GameSaveData() 
        {
            startSavePoint   = _savePointRef,
            startPointRefID  = 0,
            playerStatusSave = _playerStatusSaveSO.playerStatusSave,
            zoneSavesArray   = GetZoneSaveArray()
        } );
    }

    private ZoneSave[] GetZoneSaveArray()
    {
        ZoneSave[] zoneSaveArray = new ZoneSave[_gameZoneSavesSO.zones.Length];

        for ( int i = 0; i < _gameZoneSavesSO.zones.Length; i++ )
            zoneSaveArray[i] = _gameZoneSavesSO.zones[i].zoneSave;

        return zoneSaveArray;
    }
}
