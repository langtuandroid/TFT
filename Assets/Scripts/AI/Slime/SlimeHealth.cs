using System;
using UnityEngine;
using DG.Tweening;
using Utils;

public class SlimeHealth : MonoBehaviour, IBurnable
{
    // Esto tiene que estar en un EnemyHealth generico que se comparta por todos los enemigos
    public event Action OnDeath;

    private void OnDestroy()
    {
        OnDeath?.Invoke();
        DOTween.Kill(transform);
    }
    
    private void PlayDeathAnimation()
    {
        transform.DOScale(Vector3.zero, 1f)
            .SetEase(Ease.OutBack)
            .OnComplete(() => Destroy(gameObject)).Play();
    }

    public void Burn()
    {
        PlayDeathAnimation();
    }
}
