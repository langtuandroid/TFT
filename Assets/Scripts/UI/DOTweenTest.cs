using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DOTweenTest : MonoBehaviour
{
    public Image _image;

    private void Start()
    {
        RectTransform _rectTransform = GetComponent<RectTransform>();
        _rectTransform.DOMoveX( 0 , 8 ).Play();
        _image.DOFade( 0 , 8 ).Play();
    }
}
