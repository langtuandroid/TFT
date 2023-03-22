using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }


    private static FMOD.Studio.EventInstance _musicEventInstance;

    [SerializeField] private FMODUnity.EventReference _music;


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

    private void Init()
    {
        _musicEventInstance = FMODUnity.RuntimeManager.CreateInstance( _music );
        FMODUnity.RuntimeManager.AttachInstanceToGameObject( _musicEventInstance , transform );
        _musicEventInstance.start();

        _musicEventInstance.getVolume( out _musicVolume );
    }


    public void PlayOneShot()
    {

    }


    public void SetMusicVolume( float volume )
    {
        _musicEventInstance.setVolume( volume );
    }
    
    public void SetSfxVolume( float volume )
    {
        //_musicEventInstance.setVolume( volume );
    }
    
    public void SetUIVolume( float volume )
    {
        //_musicEventInstance.setVolume( volume );
    }
    
    public float MusicVolume() => _musicVolume;
    public float SfxVolume() => _sfxVolume;
    public float UIVolume() => _uiVolume;
}
