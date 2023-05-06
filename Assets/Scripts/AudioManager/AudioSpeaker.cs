// ************ @autor: �lvaro Repiso Romero *************
using UnityEngine;

public class AudioSpeaker
{
    private bool isDevelop = true;

    public AudioSpeaker()
    {
#if !UNITY_EDITOR
        isDevelop = false;
#endif
    }

    public void PlaySound( int groupId , int soundId )
    {
        if ( isDevelop ) return;
        AudioManager.Instance.PlayOneShot( groupId , soundId );
    }
    
    public void PlaySound( int groupId , int soundId , Vector3 soundPosition )
    {
        if ( isDevelop ) return;
        AudioManager.Instance.PlayOneShot( groupId , soundId , soundPosition );
    }
}
