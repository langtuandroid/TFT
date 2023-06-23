using System.Collections;
using UnityEngine;

public class AltarController : MonoBehaviour, IInteractable
{
    [SerializeField] private Color _completedMagicColor;
    [SerializeField] private SpriteRenderer _sealSprite;
    [SerializeField] private ZoneSaveSO[] _zoneSaveSOArray;

    [Header("To Dungeon Scene:")]
    [SerializeField] private SceneName _nextScene;
    [SerializeField] private Color _fadeOutColor;
    [SerializeField] private MusicZoneParameter _musicParamName;
    [SerializeField] private GameObject _enterIcon;

    private bool _isDungeonOpen;
    private float _fadeOutSeconds = 1f;

    private void Start()
    {
        ActivateAltar();
    }

    private void ActivateAltar()
    {
        foreach ( var zoneSaveSO in _zoneSaveSOArray )
            if ( !zoneSaveSO.zoneSave.IsCompleted )
                return;
        _isDungeonOpen = true;
        _sealSprite.color = _completedMagicColor;
    }

    public void Interact( Vector2 lookDirection )
    {
        if ( _isDungeonOpen )
        {
            ServiceLocator.GetService<IAudioSpeaker>().ChangeZoneParamater( _musicParamName , true );

            ServiceLocator.GetService<LevelEvents>().ChangeZone(
                new LevelEvents.ChangeZoneArgs
                {
                    nextStartPointRefId = 0 ,
                    fadeColor = _fadeOutColor ,
                    fadeDurationSeconds = _fadeOutSeconds
                } );

            StartCoroutine( FadeOut() );
        }
    }

    public void ShowCanInteract( bool show )
    {
        if ( _isDungeonOpen )
            _enterIcon.SetActive( show );
    }

    private IEnumerator FadeOut()
    {
        WaitForSeconds waitTime = new WaitForSeconds( _fadeOutSeconds );
        yield return waitTime;
        ServiceLocator.GetService<SceneLoader>().InstaLoad( _nextScene.ToString() );
    }
}
