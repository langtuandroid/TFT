// ************ @autor: Álvaro Repiso Romero *************
using UnityEngine;
using FMODUnity;
using Audio;

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
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if ( Input.GetKeyUp( KeyCode.Escape ) )
        {
            ChangeMusic();
        }
        
        if ( Input.GetKeyUp( KeyCode.V ) )
        {
            ChangeParameter( "Woods_Dungeon_M" , 1 );
        }
    }

    private void Init()
    {
        _musicEventInstance = RuntimeManager.CreateInstance( _gameMusicSO.MainMenu );
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
        _musicEventInstance = RuntimeManager.CreateInstance( _gameMusicSO.WoodsDungeon );
        _musicEventInstance.start();
        _musicEventInstance.release();
    }

    public void ChangeParameter( string name , float newValue )
    {
        _musicEventInstance.setParameterByName( name , newValue );
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
