using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DOTFadeInImage : MonoBehaviour
{
    private Image _spriteRenderer;
    public float fadeInDuration = 1f;
    public float delay = 0f;

    private void Awake()
    {
        _spriteRenderer = GetComponent<Image>();
    }

    void Start()
    {
        Color spriteColor = _spriteRenderer.color;
        spriteColor.a = 0f;
        StartCoroutine(nameof(Delay));
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(delay);
        
        Color spriteColor = _spriteRenderer.color;
        spriteColor.a = 0f;
        _spriteRenderer.color = spriteColor;
        
        transform.DOScale(5, 1).SetEase(Ease.OutQuad).Play();
        _spriteRenderer.DOFade(0.2f, fadeInDuration).Play();
        
    }
}
