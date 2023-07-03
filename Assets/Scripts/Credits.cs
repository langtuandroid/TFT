using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public float delay = 1f;
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 1f;
    public float moveDistance = 200f;
    public float moveDuration = 2f;

    private void Start()
    {
        AnimateCredit();
    }

    private void AnimateCredit()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
            child.GetComponent<CanvasGroup>().alpha = 0f;
            child.transform.localPosition -= new Vector3(0f, moveDistance, 0f);
        }
        
        for (int i = 0; i < transform.childCount; i++)
        {
            Sequence sequence = DOTween.Sequence();
            Transform child = transform.GetChild(i);
            float delay = i * (moveDuration + fadeInDuration + fadeOutDuration);
            sequence.AppendInterval(delay);
            sequence.AppendCallback(() => child.gameObject.SetActive(true));
            sequence.Append(child.GetComponent<CanvasGroup>().DOFade(1f, fadeInDuration));
            sequence.Append(child.transform.DOLocalMoveY(child.transform.localPosition.y + moveDistance, moveDuration));
            sequence.AppendInterval(1);
            sequence.Append(child.GetComponent<CanvasGroup>().DOFade(0f, fadeOutDuration));

            if (i == transform.childCount - 1)
            {
                sequence.AppendCallback(() =>
                {
                    StartCoroutine(nameof(BackToMenu));
                });        
            }
            
            sequence.Play();
        }
        

    }

    private IEnumerator BackToMenu()
    {
        yield return new WaitForSeconds(1f);
        
        ServiceLocator.GetService<SceneLoader>().Load(SceneName.S00_MainMenuScene.ToString());
    }
}
