// ************ @autor: Álvaro Repiso Romero *************
using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }


    [SerializeField] private EventReference _fireBallCast;
    [SerializeField] private EventReference _tecnoMusic;

    [Header("Music currently playing")]
    private static FMOD.Studio.EventInstance _musicEventInstance;

    [Header("Volumes")]
    private FMOD.Studio.Bus _musicMixer;
    private FMOD.Studio.Bus _sfxMixer;
    private FMOD.Studio.Bus _uiMixer;
    private float _musicVolume;
    private float _sfxVolume;
    private float _uiVolume;


    private void Awake()
    {
        if ( Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeMusic();
        }
    }

    private void Init()
    {
        _musicEventInstance = RuntimeManager.CreateInstance( _tecnoMusic );
        RuntimeManager.AttachInstanceToGameObject( _musicEventInstance , transform );
        _musicEventInstance.start();

        _musicMixer = RuntimeManager.GetBus( "bus:/Music" );
        _sfxMixer = RuntimeManager.GetBus( "bus:/Sfx" );
        //_uiMixer = RuntimeManager.GetBus( "bus:/UI" );

        _musicMixer.getVolume( out _musicVolume );
        _sfxMixer.getVolume( out _sfxVolume );
        //_uiMixer.getVolume( out _uiVolume );
    }

    private void ChangeMusic()
    {
        _musicEventInstance.stop( FMOD.Studio.STOP_MODE.ALLOWFADEOUT );
        _musicEventInstance = RuntimeManager.CreateInstance( _fireBallCast );
        _musicEventInstance.start();
    }


    public void PlayOneShot()
    {
        RuntimeManager.PlayOneShot( _fireBallCast );
    }

    public void PlayOneShot( Vector3 soundOrigin )
    {
        RuntimeManager.PlayOneShot( _fireBallCast , soundOrigin );
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
    
    public void SetUIVolume( float volume )
    {
        _uiVolume = volume;
        //_uiMixer.setVolume( volume );
    }
    
    public float MusicVolume() => _musicVolume;
    public float SfxVolume() => _sfxVolume;
    public float UIVolume() => _uiVolume;
}
