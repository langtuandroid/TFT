using UnityEngine;

public class MusicChanger : MonoBehaviour
{
    private void Awake()
    {
        ServiceLocator.GetService<IAudioSpeaker>().ChangeMusic( MusicName.Woods_Dungeon );
    }
    
    private void OnDestroy()
    {
        ServiceLocator.GetService<IAudioSpeaker>().ChangeMusic( MusicName.Main_Menu );
    }
}
