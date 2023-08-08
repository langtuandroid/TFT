using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Assertions.Must;
using Utils;


public class GolemHealth : MonoBehaviour, IBurnable, IPunchanble
{
    public event Action OnDeath;
    public event Action OnDamage; 
    private int _maxPhisicalDamage = 10;
    private bool _canReceiveDamage = true;
    private SpriteRenderer spriteRenderer;
    private float _damage = 2;
    private float _handDetectionRadious = 1f;

    public float Damage 
    {
        get => _damage;
    }
    
    private void OnDestroy()
    {
        OnDeath?.Invoke();
        DOTween.Kill(transform);
    }
    
    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        CheckArmCollision();
    }

    private void PlayDeathAnimation()
    {
        transform.DOScale(Vector3.zero, 1f)
            .SetEase(Ease.OutBack)
            .OnComplete(() => Destroy(gameObject)).Play();
    }

    public void PlayDamageAnimation(float duration, int cant)
    {
        OnDamage?.Invoke();
        
        spriteRenderer.DOColor(Color.clear, duration / (cant * 2f))
            .SetLoops(cant * 2, LoopType.Yoyo)
            .OnComplete(() => _canReceiveDamage = true).Play();
    }
    
    private void CheckArmCollision()
    {
        Collider2D results = Physics2D.OverlapCircle(transform.position, _handDetectionRadious);

        if (results != null)
        {
            if (results.CompareTag("GolemArm") && _canReceiveDamage)
            {
                results.GetComponent<GolemRockHealth>().GolemCollision();
                
                _canReceiveDamage = false;
            
                _damage--;

                if (_damage > 0) PlayDamageAnimation(2f, 3);
                else PlayDeathAnimation();
            }
        }
    }

    public void Burn(int damage)
    {
      /*  _maxPhisicalDamage -= damage;
        
        Debug.Log("le queda de vida: " + _maxPhisicalDamage);
        
        if (_maxPhisicalDamage <= 0f)
            PlayDeathAnimation();
        else
            PlayDamageAnimation(2f, 3);*/
    }

    public void Punch(int damage)
    {
      /*  if (!_canReceiveDamage) return;
        _canReceiveDamage = false;
        
        _maxPhisicalDamage -= damage;
        
        Debug.Log("le queda de vida: " + _maxPhisicalDamage);
        
        if (_maxPhisicalDamage <= 0f)
            PlayDeathAnimation();
        else
            PlayDamageAnimation(2f, 3);*/
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _handDetectionRadious);
    }
}
