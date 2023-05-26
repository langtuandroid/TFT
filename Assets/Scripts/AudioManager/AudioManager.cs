// ************ @autor: Álvaro Repiso Romero *************
using UnityEngine;
using FMODUnity;
using Audio;
using System.Collections;

[DefaultExecutionOrder(-10)]
public class AudioManager : MonoBehaviour, IAudioSpeaker
{
    public static AudioManager Instance { get; private set; }


    [SerializeField] private GameMusicSO _gameMusicSO;
    [SerializeField] private SfxGroupSO  _sfxGroupSO;

    [Header("Music currently playing")]
    private static FMOD.Studio.EventInstance _musicEventInstance;

    [Header("Volumes")]
    private FMOD.Studio.Bus _musicMixer;
    private FMOD.Studio.Bus _sfxMixer;
    private float _musicVolume;
    private float _sfxVolume;


    private void Awake()
    {
        if ( Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad( gameObject );
            Init();
        }
        else
            Destroy(gameObject);
    }

    private void Init()
    {
        _musicMixer = RuntimeManager.GetBus( "bus:/Music" );
        _sfxMixer   = RuntimeManager.GetBus( "bus:/Sfx" );

        _musicMixer.getVolume( out _musicVolume );
        _sfxMixer.getVolume( out _sfxVolume );
    }

    public void StartMusic()
    {
        _musicEventInstance = RuntimeManager.CreateInstance( _gameMusicSO.gameMusic[( int )MusicName.Main_Menu] );
        _musicEventInstance.start();
        _musicEventInstance.release();
    }


    public void ChangeMusic( MusicName musicId )
    {
        _musicEventInstance.stop( FMOD.Studio.STOP_MODE.ALLOWFADEOUT );
        StartCoroutine( StartMusic( (int)musicId ) );
    }

    private IEnumerator StartMusic( int musicId )
    {
        WaitForSeconds wait = new WaitForSeconds( 1.5f );
        yield return wait;
        _musicEventInstance = RuntimeManager.CreateInstance( _gameMusicSO.gameMusic[musicId] );
        _musicEventInstance.start();
        _musicEventInstance.release();
    }


    public void ChangeParamater( MusicParameterName paramName , bool isActivatingParam )
    {
        StartCoroutine( ChangeParam( paramName.ToString() , isActivatingParam ) );
    }

    private IEnumerator ChangeParam( string paramName , bool isActivatingParam )
    {
        float acumulated = 0;
        while ( acumulated < 1 )
        {
            _musicEventInstance.setParameterByName( paramName , isActivatingParam ? acumulated : 1 - acumulated );

            acumulated += Time.deltaTime;
            yield return null;
        }
        _musicEventInstance.setParameterByName( paramName , isActivatingParam ? 1 : 0 );
    } 


    public void PlaySound( int groupId, int soundId , Vector3 soundPosition = new() )
    {
        Debug.Log( "[Play Sound]: " + _sfxGroupSO.list[groupId].SfxRef[soundId].SoundName + 
            ", IDSend: " + soundId + " = ID: " + _sfxGroupSO.list[groupId].SfxRef[soundId].Id );
        RuntimeManager.PlayOneShot( _sfxGroupSO.list[groupId].SfxRef[soundId].Sound , soundPosition );
    }


    public void SetMusicVolume( float volume )
    {
        _musicVolume = volume;
        _musicMixer.setVolume( volume );
    }
    
    public void SetSfxVolume( float volume )
    {
        _sfxVolume = volume;
        _sfxMixer.setVolume( volume );
    }
    
    public float MusicVolume() => _musicVolume;
    public float SfxVolume() => _sfxVolume;
}
