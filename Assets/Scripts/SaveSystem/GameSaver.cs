// ************ @autor: Álvaro Repiso Romero *************
using UnityEngine;

public class GameSaver : MonoBehaviour, IInteractable
{
    [SerializeField][Range( 0, 3 )] private int _savePointRef;
    [SerializeField] private GameObject _exclamationIcon;
    [SerializeField] private ZoneSaveSO[] _zoneSaveSOArray;

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
    }
}
