using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class SlimeHealth : MonoBehaviour, IBurnable, IPunchanble
{
    // Esto tiene que estar en un EnemyHealth generico que se comparta por todos los enemigos
    public event Action OnDeath;
    [SerializeField ]private int _maxPhisicalDamage = 2;// Daño con la vara
    [SerializeField] private int _maxDamage = 1;//Daño con magia
    
    private bool _canReceiveDamage = true;
    private SpriteRenderer spriteRenderer;
    private Animator _animator;
    private Collider2D _collider2D;
    
    private void OnDestroy()
    {
        DOTween.Kill(transform);
    }
    
    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _collider2D = GetComponentInParent<Collider2D>();
    }
    
    private void PlayDeathAnimation()
    {
        OnDeath?.Invoke();
        _collider2D.enabled = false;
        _animator.Play("Slime_Death");
        
        float deathAnimationDuration = _animator.GetCurrentAnimatorStateInfo(0).length;
        
        Debug.Log("Animacion: " + deathAnimationDuration);
        StartCoroutine(WaitAndInvokeDOTDead(deathAnimationDuration));
    }

    private IEnumerator WaitAndInvokeDOTDead(float delay)
    {
        yield return new WaitForSeconds(delay);
        DOTDead();
    }


    private void DOTDead()
    {
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
        
        _maxDamage -= damage;
        
        if (_maxDamage <= 0f)
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
