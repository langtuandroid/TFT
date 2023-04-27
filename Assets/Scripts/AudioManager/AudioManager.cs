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
    private float _musicVolume;
    private float _sfxVolume;


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
        _musicEventInstance.start();
        _musicEventInstance.release();

        _musicMixer = RuntimeManager.GetBus( "bus:/Music" );
        _sfxMixer = RuntimeManager.GetBus( "bus:/Sfx" );

        _musicMixer.getVolume( out _musicVolume );
        _sfxMixer.getVolume( out _sfxVolume );
    }

    private void ChangeMusic()
    {
        _musicEventInstance.stop( FMOD.Studio.STOP_MODE.ALLOWFADEOUT );
        _musicEventInstance = RuntimeManager.CreateInstance( _fireBallCast );
        _musicEventInstance.start();
    }

    public void PlayOneShot( Vector3 soundOrigin = new() )
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
    
    public float MusicVolume() => _musicVolume;
    public float SfxVolume() => _sfxVolume;
}
