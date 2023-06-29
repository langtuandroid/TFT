// ************ @autor: Álvaro Repiso Romero *************
using UnityEngine;
using FMODUnity;
using Audio;
using System.Collections;
using System.Collections.Generic;

[DefaultExecutionOrder(-10)]
public class AudioManager : MonoBehaviour, IAudioSpeaker
{
    public static AudioManager Instance { get; private set; }


    [SerializeField] private GameMusicSO _gameMusicSO;
    [SerializeField] private SfxGroupSO  _sfxGroupSO;

    [Header("Music currently playing")]
    private FMOD.Studio.EventInstance _musicEventInstance;
    private MusicZoneParameter _currentZoneParameter;

    [Header("Volumes")]
    private FMOD.Studio.Bus _musicMixer;
    private FMOD.Studio.Bus _sfxMixer;
    private float _musicVolume;
    private float _sfxVolume;

    [Header("Music List")]
    private Dictionary<MusicName , EventReference> _gameMusicDict;

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

        _currentZoneParameter = MusicZoneParameter.None;

        _gameMusicDict = _gameMusicSO.GameMusicDictionary();

        //StartMusic();
    }

    public void StartMusic()
    {
        _musicEventInstance = RuntimeManager.CreateInstance( _gameMusicDict[MusicName.Main_Menu] );
        _musicEventInstance.start();
        _musicEventInstance.release();
    }


    public void ChangeMusic( MusicName musicName )
    {
        if ( musicName.Equals( MusicName.None ) ) return;
        _musicEventInstance.stop( FMOD.Studio.STOP_MODE.ALLOWFADEOUT );
        StartCoroutine( MusicStarter( musicName ) );
    }

    private IEnumerator MusicStarter( MusicName musicName )
    {
        WaitForSeconds wait = new WaitForSeconds( 1.5f );
        yield return wait;
        _musicEventInstance = RuntimeManager.CreateInstance( _gameMusicDict[musicName] );
        _musicEventInstance.start();
        _musicEventInstance.release();
    }


    public void ChangeZoneParamater( MusicZoneParameter paramName , bool isActivatingParam )
    {
        if ( _currentZoneParameter.Equals( paramName ) )
            return;
        StartCoroutine( ChangeParam( paramName ) );
    }

    private IEnumerator ChangeParam( MusicZoneParameter paramName )
    {
        float acumulated = 1;
        string paramNameStr = _currentZoneParameter.ToString();
        Debug.Log( paramNameStr );

        if ( !_currentZoneParameter.Equals( MusicZoneParameter.None ) )
        {
            Debug.Log( "1st in" );
            while ( acumulated > 0 )
            {
                _musicEventInstance.setParameterByName( paramNameStr , acumulated );

                acumulated -= Time.deltaTime;
                yield return null;
            }
            _musicEventInstance.setParameterByName( paramNameStr , 0 );
        }

        _currentZoneParameter = paramName;
        if ( !paramName.Equals( MusicZoneParameter.None ) )
        {
            Debug.Log( "2nd in" );
            paramNameStr = paramName.ToString();
            Debug.Log( paramNameStr );
            acumulated = 0;

            while ( acumulated < 1 )
            {
                _musicEventInstance.setParameterByName( paramNameStr , acumulated );

                acumulated += Time.deltaTime;
                yield return null;
            }
            _musicEventInstance.setParameterByName( paramNameStr , 1 );
        }
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
