using System.Collections;
using UnityEngine;

public class AltarController : MonoBehaviour, IInteractable
{
    [SerializeField] private AltarColorSO _altarColorSO;
    [SerializeField] private SpriteRenderer _sealSprite;
    [SerializeField] private ZoneSaveSO[] _zoneSaveSOArray;

    [Header("To Dungeon Scene:")]
    [SerializeField] private ChangeSceneInfoSO _changeSceneInfoSO;
    [SerializeField] private GameObject _enterIcon;

    private bool _isDungeonOpen;

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
        _sealSprite.color = _altarColorSO.completeMagicColor;
    }

    public void Interact( Vector2 lookDirection )
    {
        if ( _isDungeonOpen )
        {
            ServiceLocator.GetService<IAudioSpeaker>().ChangeMusic( _changeSceneInfoSO.MusicName );

            ServiceLocator.GetService<LevelEvents>().ChangeZone(
                new LevelEvents.ChangeZoneArgs
                {
                    nextStartPointRefId = 0 ,
                    fadeColor = _changeSceneInfoSO.FadeOutColor ,
                    fadeDurationSeconds = _changeSceneInfoSO.FadeOutSeconds
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
        WaitForSeconds waitTime = new WaitForSeconds( _changeSceneInfoSO.FadeOutSeconds );
        yield return waitTime;
        ServiceLocator.GetService<SceneLoader>().Load( _changeSceneInfoSO.NextScene.ToString() );
    }
}
