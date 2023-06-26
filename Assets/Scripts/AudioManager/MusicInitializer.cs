using UnityEngine;

public class MusicInitializer : MonoBehaviour
{
    private void OnDestroy()
    {
        ServiceLocator.GetService<IAudioSpeaker>().StartMusic();
    }
}
