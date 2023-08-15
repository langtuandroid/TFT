using System;
using UnityEngine;
using DG.Tweening;
public class GolemRockHealth : MonoBehaviour, IBurnable, IPunchable
{
    [SerializeField] private Transform ThrowPosition;
    [SerializeField] private Transform _golemPosition;
    [SerializeField] private float punchForce;
    
    public event Action OnDeath;
    private int _maxPhisicalDamage = 1;
    private bool _canReceiveDamage = true;
    private SpriteRenderer spriteRenderer;
    private SpringJoint2D _joint2D;
    private Rigidbody2D _rb;
    private GolemArmIA _golemArmIA;
    private bool _isArmDefeated = false;
    private Collider2D _collider2D;
    
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
        _golemArmIA = GetComponent<GolemArmIA>();
        _collider2D = GetComponent<Collider2D>();
    }
    
    private void PlayDeathAnimation()
    {
        _golemArmIA.enabled = false;
        _rb.velocity = Vector2.zero;
        _joint2D.enabled = false;
        transform.position = ThrowPosition.position;
        transform.SetParent(ThrowPosition);
        _isArmDefeated = true;
    }

    private void PlayDamageAnimation(float duration, int cant)
    {
        spriteRenderer.DOColor(Color.clear, duration / (cant * 2f))
            .SetLoops(cant * 2, LoopType.Yoyo)
            .OnComplete(() => _canReceiveDamage = true).Play();
    }
    
    private void PunchDefeatedArm()
    {
        _collider2D.isTrigger = true;
        Vector2 direction = (_golemPosition.position - transform.position).normalized;
        _rb.velocity = direction * punchForce; 
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
        if (_isArmDefeated)
        {
            PunchDefeatedArm(); 
        }
        else
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

    public void GolemCollision()
    {
        transform.DOScale(Vector3.zero, 1f)
            .SetEase(Ease.OutBack)
            .OnComplete(() => Destroy(gameObject)).Play();
    }
}
