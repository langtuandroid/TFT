// ************ @autor: Álvaro Repiso Romero *************
using UnityEngine;

public class AudioSpeaker
{
    private bool isDevelop = true;

    public void PlaySound( int groupId , int soundId )
    {
        Debug.Log( "El grupo: " + GroupIdToName( groupId ) + " hizo sonar: " + SoundIdToName( soundId ) );
        if ( isDevelop ) return;
        AudioManager.Instance.PlayOneShot( groupId , soundId );
    }
    
    public void PlaySound( int groupId , int soundId , Vector3 soundPosition )
    {
        Debug.Log( "El grupo: " + GroupIdToName( groupId ) + " hizo sonar: " + SoundIdToName( soundId ) );
        if ( isDevelop ) return;
        AudioManager.Instance.PlayOneShot( groupId , soundId , soundPosition );
    }

    private string GroupIdToName( int groupId )
    {
        switch ( groupId )
        {
            case AudioID.G_PLAYER:
                return "e";
        }
        return "Peaso Error en el grupoId nene";
    }
    
    private string SoundIdToName( int soundId )
    {
        switch ( soundId )
        {

        }
        return "Peaso Error en el soundId nene";
    }
}
