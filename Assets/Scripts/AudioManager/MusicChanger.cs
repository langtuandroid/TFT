using UnityEngine;

public class MusicChanger : MonoBehaviour
{
    private void OnDestroy()
    {
        ServiceLocator.GetService<IAudioSpeaker>().ChangeMusic( MusicName.Main_Menu );
    }
}
