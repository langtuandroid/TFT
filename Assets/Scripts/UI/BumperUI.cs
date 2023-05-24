using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BumperUI : MonoBehaviour
{
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Start()
    {
        ServiceLocator.GetService<LevelEvents>().OnChangeZone += FadeOut;
        FadeIn();
    }

    private void OnDestroy()
    {
        ServiceLocator.GetService<LevelEvents>().OnChangeZone -= FadeOut;        
    }

    private void FadeIn()
    {
        _image.color = Color.white;
        _image.DOFade( 0 , 1 ).Play();
    }

    private void FadeOut( TriggerChangeZone.FadeOutArgs bumperUIArgs )
    {
        Color startColor = new Color( bumperUIArgs.fadeColor.r , 
                                      bumperUIArgs.fadeColor.g , 
                                      bumperUIArgs.fadeColor.b , 0 );
        _image.color = startColor;
        _image.DOFade( 1 , bumperUIArgs.fadeDurationSeconds ).Play();    
    }
}
