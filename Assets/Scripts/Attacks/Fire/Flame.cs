using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utils;
using FMOD.Studio;

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

    #endregion

    #region Unity methods

    private void Start()
    {
        MoveAndGrow();
    }

    private void FixedUpdate()
    {
        CheckCollisions();
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
        RaycastHit2D[] hits = Physics2D.CircleCastAll(
            transform.position,
            transform.lossyScale.x * .5f,
            Vector2.zero,
            transform.lossyScale.x * .5f,
            LayerMask.GetMask("Interactable")
            );

        if (hits.Length > 0)
            foreach (RaycastHit2D hit in hits)
                hit.transform.gameObject.GetComponent<IBurnable>()?
                    .Burn(_damage);


    }

    #endregion

}
