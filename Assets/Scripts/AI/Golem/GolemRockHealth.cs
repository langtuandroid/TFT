using System;
using UnityEngine;
using DG.Tweening;
public class GolemRockHealth : MonoBehaviour, IBurnable, IPunchanble
{
    public Transform ThrowPosition;
    public event Action OnDeath;
    private int _maxPhisicalDamage = 3;
    private bool _canReceiveDamage = true;
    private SpriteRenderer spriteRenderer;
    private SpringJoint2D _joint2D;
    private Rigidbody2D _rb;
    
    private void OnDestroy()
    {
        OnDeath?.Invoke();
        DOTween.Kill(transform);
    }
    
    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _joint2D = GetComponent<SpringJoint2D>();
        _rb = GetComponent<Rigidbody2D>();
    }
    
    private void PlayDeathAnimation()
    {
        _rb.isKinematic = true;
        _joint2D.enabled = false;
        transform.SetParent(ThrowPosition);
        transform.position = new Vector3(0f, 0f, 0f);
    }

    private void PlayDamageAnimation(float duration, int cant)
    {
        spriteRenderer.DOColor(Color.clear, duration / (cant * 2f))
            .SetLoops(cant * 2, LoopType.Yoyo)
            .OnComplete(() => _canReceiveDamage = true).Play();
    }

    public void Burn(int damage)
    {
        _maxPhisicalDamage -= damage;
        
        Debug.Log("le queda de vida: " + _maxPhisicalDamage);
        
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
        
        Debug.Log("le queda de vida: " + _maxPhisicalDamage);
        
        if (_maxPhisicalDamage <= 0f)
            PlayDeathAnimation();
        else
            PlayDamageAnimation(2f, 3);
    }
}
