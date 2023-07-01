using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DOTCinematicText : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        string originalText = textMesh.text;
        
        textMesh.text = "";
        
        DOTween.Sequence()
            .SetDelay(0.5f)
            .AppendCallback(() => AnimateText(originalText))
            .Play();
    }

    private void AnimateText(string originalText)
    {
        for (int i = 0; i < originalText.Length; i++)
        {
            int currentIndex = i;
            
            DOTween.Sequence()
                .AppendInterval(0.1f * currentIndex)
                .AppendCallback(() =>
                {
                    textMesh.text += originalText[currentIndex];
                    textMesh.transform.GetChild(currentIndex).DOScale(Vector3.one * 1.5f, 0.2f)
                        .SetLoops(2, LoopType.Yoyo);
                })
                .Play();
        }
    }
}
