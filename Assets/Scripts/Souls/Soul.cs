using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Soul : MonoBehaviour
{
    #region SerializeFields

    [Header("Settings")]
    [SerializeField]
    [Tooltip("Layer del jugador")]
    private LayerMask _playerMask;
    [SerializeField]
    [Tooltip("Distancia para empezar a detectar al jugador")]
    private float _distanceToDetect = 4f;
    [SerializeField]
    [Tooltip("Velocidad de aproximación hacia el jugador")]
    private float _speed = 40f;

    [SerializeField]
    [Tooltip("Cantidad de almas que da al player")]
    private int _value = 1;

    #endregion

    #region Private variables

    // EVENTS
    private SoulEvents _soulEvents; // Evento para las almas

    // COMPONENTS
    private Rigidbody2D _rb; // RigidBody2D del GameObject

    // VARIABLES
    private Vector3 _playerPosition; // Dirección hacia la que se va a dirigir

    #endregion

    #region Unity methods

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _soulEvents = ServiceLocator.GetService<SoulEvents>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si colisiona con el player
        if (collision.CompareTag(Constants.TAG_PLAYER))
        {
            // Damos la cantidad correspondiente de almas
            _soulEvents.GotSouls(_value);
            // Y destruimos el objeto
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Detectamos al jugador
        DetectPlayer();
    }

    private void FixedUpdate()
    {
        // Si no ha detectado al jugador,
        // volvemos
        if (_playerPosition == Vector3.zero)
            return;

        // Nos movemos al jugador
        MoveToPlayer(_playerPosition - transform.position);
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Se encarga de detectar al jugador
    /// </summary>
    private void DetectPlayer()
    {
        Collider2D collider = Physics2D.OverlapCircle(
            transform.position,
            _distanceToDetect,
            _playerMask
            );

        _playerPosition = collider == null ? Vector2.zero :
            collider.transform.position;
    }

    private void MoveToPlayer(Vector2 direction)
    {
        // Cogemos el vector dirección como magnitud
        Vector2 dir = direction.normalized;
        // Y lo movemos (a mayor distancia, más lentitud de acercamiento)
        _rb.MovePosition(_rb.position +
            Time.deltaTime * _speed / direction.magnitude * dir);
    }

    #endregion

}
