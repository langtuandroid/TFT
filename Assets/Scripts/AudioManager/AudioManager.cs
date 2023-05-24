// ************ @autor: Álvaro Repiso Romero *************
using UnityEngine;
using FMODUnity;
using Audio;
using System.Collections;

public class AudioManager : MonoBehaviour
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
        _musicEventInstance = RuntimeManager.CreateInstance( _gameMusicSO.Woods );
        _musicEventInstance.start();
        _musicEventInstance.release();

        _musicMixer = RuntimeManager.GetBus( "bus:/Music" );
        _sfxMixer   = RuntimeManager.GetBus( "bus:/Sfx" );

        _musicMixer.getVolume( out _musicVolume );
        _sfxMixer.getVolume( out _sfxVolume );
    }

    public void ChangeMusic()
    {
        _musicEventInstance.stop( FMOD.Studio.STOP_MODE.ALLOWFADEOUT );
        _musicEventInstance = RuntimeManager.CreateInstance( _gameMusicSO.MainMenu );
        _musicEventInstance.start();
        _musicEventInstance.release();
    }

    public void ChangeParameter( MusicParameterName paramName , float newValue )
    {
        StartCoroutine( ChangeParam( paramName.ToString() , newValue ) );
    }

    private IEnumerator ChangeParam( string paramName , float newValue )
    {
        float acumulated = 0;
        while ( acumulated < newValue )
        {
            _musicEventInstance.setParameterByName ( paramName , newValue );

            acumulated += Time.deltaTime;
            yield return null;
        }
        _musicEventInstance.setParameterByName ( paramName , newValue );
    } 

    public void PlayOneShot( int groupId, int soundId , Vector3 soundOrigin = new() )
    {
        Debug.Log( "[Play Sound]: " + _sfxGroupSO.list[groupId].SfxRef[soundId].SoundName + 
            ", IDSend: " + soundId + " = ID: " + _sfxGroupSO.list[groupId].SfxRef[soundId].Id );
        RuntimeManager.PlayOneShot( _sfxGroupSO.list[groupId].SfxRef[soundId].Sound , soundOrigin );
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
