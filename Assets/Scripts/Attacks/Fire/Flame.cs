using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utils;

public class Flame : MonoBehaviour
{
    #region SerializeFields

    [Header("Flame settings")]
    [SerializeField]
    [Tooltip("Velocidad de movimiento de la llama")]
    private float _velocity = 4f;
    [SerializeField]
    [Tooltip("Tiempo de vida de la llama")]
    private float _lifeTime = 1f;
    [SerializeField]
    [Tooltip("Daño que produce la llama (como es gradual, es mejor ponerlo bajo)")]
    private int _damage = 1;

    #endregion


    #region Private variables

    // VARIABLES
    private Vector2 _direction; // Movement direction

    private List<IBurnable> _burnables; // List of collisions

    #endregion

    #region Unity methods

    private void Start()
    {
        _burnables = new List<IBurnable>();
        MoveAndGrow();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IBurnable>(out IBurnable burn))
            _burnables.Add(burn);

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IBurnable>(out IBurnable burn))
            _burnables.Remove(burn);
    }

    private void Update()
    {
        CheckCollisions();
    }

    private void OnDestroy()
    {
        _burnables.Clear();
    }

    #endregion

    #region Public methods

    public void Init(Vector2 direction)
    {
        _direction = direction;
    }

    #endregion

    #region Private methods

    private void MoveAndGrow()
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Tween t = transform.DOMove(pos + (_direction * _velocity * _lifeTime * .8125f), _lifeTime);
        Tween t2 = transform.DOScale(2, _lifeTime);
        t.Play();
        t2.Play().OnComplete(() => Destroy(gameObject));
    }

    private void CheckCollisions()
    {
        foreach (IBurnable burn in _burnables)
            burn.Burn(_damage);

    }

    #endregion

}
