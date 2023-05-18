using System;
using UnityEngine;
using DG.Tweening;
using Utils;

public class SlimeHealth : MonoBehaviour
{
    // Esto tiene que estar en un EnemyHealth generico que se comparta por todos los enemigos
    public event Action OnDeath;

    private void OnDestroy()
    {
        OnDeath?.Invoke();
        DOTween.Kill(transform);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(Constants.TAG_MAGIC_POWER))
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
