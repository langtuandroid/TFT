// ************ @autor: Álvaro Repiso Romero *************
using UnityEngine;

public interface IAudioSpeaker
{
    public void ChangeMusic( MusicName musicId );
    public void ChangeZoneParamater( MusicZoneParameter paramName , bool isActivatingParam );
    public void PlaySound( int groupId , int soundId , Vector3 soundPosition = new() );
    public void SetMusicVolume( float volume );
    public void SetSfxVolume( float volume );
    public float MusicVolume();
    public float SfxVolume();
}
public class DummyAudio : IAudioSpeaker
{
    public void ChangeMusic( MusicName musicId ) { }
    public void ChangeZoneParamater( MusicZoneParameter paramName , bool isActivatingParam ) { }
    public float MusicVolume() { return 0; }
    public void PlaySound( int groupId , int soundId , Vector3 soundPosition = default ) { }
    public void SetMusicVolume( float volume ) { }
    public void SetSfxVolume( float volume ) { }
    public float SfxVolume() { return 0; }
}