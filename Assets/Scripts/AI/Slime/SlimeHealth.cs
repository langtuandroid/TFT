using System;
using UnityEngine;
using DG.Tweening;

public class SlimeHealth : MonoBehaviour, IBurnable, IPunchable
{
    // Esto tiene que estar en un EnemyHealth generico que se comparta por todos los enemigos
    public event Action OnDeath;
    public int _maxPhisicalDamage = 2;
    private bool _canReceiveDamage = true;
    private SpriteRenderer spriteRenderer;
    private EnemyDamage _enemyDamage;
    
    private void OnDestroy()
    {
        OnDeath?.Invoke();
        DOTween.Kill(transform);
    }
    
    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _enemyDamage = GetComponent<EnemyDamage>();
    }
    
    private void PlayDeathAnimation()
    {
        _enemyDamage.enabled = false; //TODO revisar, no esta funcionando
        
        transform.DOScale(Vector3.zero, 1f)
            .SetEase(Ease.OutBack)
            .OnComplete(() => Destroy(gameObject)).Play();
    }

    private void PlayDamageAnimation(float duration, int cant)
    {
        spriteRenderer.DOColor(Color.clear, duration / (cant * 2f))
            .SetLoops(cant * 2, LoopType.Yoyo)
            .OnComplete(() => _canReceiveDamage = true).Play();
    }

    public void Burn(int damage)
    {
        if (!_canReceiveDamage) return;
        _canReceiveDamage = false;
        
        _maxPhisicalDamage -= damage;
        
        if (_maxPhisicalDamage <= 0f)
            PlayDeathAnimation();
        else
            PlayDamageAnimation(2f, 3);
    }

    public void Punch(int damage)
    {
        if (!_canReceiveDamage) return;
         _canReceiveDamage = false;
        
        _maxPhisicalDamage -= damage;
        
        if (_maxPhisicalDamage <= 0f)
            PlayDeathAnimation();
        else
            PlayDamageAnimation(2f, 3);
    }
}
