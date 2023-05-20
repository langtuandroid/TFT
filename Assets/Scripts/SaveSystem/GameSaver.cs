using UnityEngine;

public class GameSaver : MonoBehaviour, IInteractable
{
    [SerializeField][Range( 0, 3 )] private int _savePointRef;
    [SerializeField] private GameObject _exclamationIcon;
    [SerializeField] private ZoneSaveSO[] _zoneSaveSOArray;

    public void Interact( Vector2 lookDirection )
    {
        if ( lookDirection.y > 0 )
        {
            Debug.Log( "Saving..." );
            SaveData();
        }
        else
            Debug.Log( "Cannot Save From Here" );
    }

    public void ShowCanInteract( bool show )
    {
        _exclamationIcon.SetActive( show );
    }

    private void SaveData()
    {
        OptionsSave optionsSave = ServiceLocator.GetService<OptionsSave>();
    }
}
