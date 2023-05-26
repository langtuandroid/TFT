using UnityEngine;

public class MusicInitializer : MonoBehaviour
{
    private void OnDestroy()
    {
        AudioManager.Instance.StartMusic();
    }
}
