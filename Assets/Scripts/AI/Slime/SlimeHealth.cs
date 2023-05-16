
using UnityEngine;
using DG.Tweening;
using Utils;

public class SlimeHealth : MonoBehaviour
{
    private void OnDestroy()
    {
        DOTween.Kill(transform);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(Constants.TAG_FIRE_BALL))
        {
            PlayDeathAnimation();
        }
    }

    private void PlayDeathAnimation()
    {
        transform.DOScale(Vector3.zero, 1f)
            .SetEase(Ease.OutBack)
            .OnComplete(() => Destroy(gameObject)).Play();
    }
}
