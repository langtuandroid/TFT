using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour, IBurnable, IInteractable
{
    #region SerializeField

    [SerializeField]
    [Tooltip("Niveles de planta (de menor a mayor)")]
    private List<GameObject> _levels;

    [SerializeField]
    [Tooltip("Dirección de la planta")]
    private Direction _direction;

    [SerializeField]
    [Tooltip("Tiempo para recuperarse")]
    private float _timeToRestore = 5f;

    #endregion

    #region Private variables

    // Elementos
    private BoxCollider2D _collider;
    // Variables
    private int _lifes;
    private Coroutine _coroutine;

    #endregion

    #region Private class

    private enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    #endregion


    #region Unity methods

    private void Awake()
    {
        // Recogemos componentes
        _collider = GetComponent<BoxCollider2D>();

        // Inicializamos variables
        _lifes = _levels.Count - 1;
        _levels[_lifes].SetActive(true);

        // Cambiamos el collider
        ChangeCollider();
    }

    #endregion

    #region Public methods

    public void Burn()
    {

        // Desactivamos corrutina
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        // Y si es el último toque
        if (_lifes == 0)
        {
            // Volvemos a activar corrutina
            ActivateCoroutine();
            // Y volvemos
            return;
        }

        // Desactivamos la planta actual
        _levels[_lifes].SetActive(false);
        // Se reduce la vida
        _lifes--;
        // Y activamos el nivel actual
        _levels[_lifes].SetActive(true);
        // Cambiamos el collider
        ChangeCollider();

        // Finalmente, activamos corrutina
        ActivateCoroutine();
    }

    public void Interact(Vector2 lookDirection)
    {
        Debug.Log("Maybe I can burn it");
    }

    public void ShowCanInteract(bool show)
    {
    }

    #endregion

    #region Private methods

    #region Coroutines

    /// <summary>
    /// Corrutina para ir recuperando el tamaño original
    /// </summary>
    /// <returns></returns>
    private IEnumerator RestoreSize()
    {
        yield return new WaitForSeconds(_timeToRestore);
        _levels[_lifes].SetActive(false);
        _lifes++;
        _levels[_lifes].SetActive(true);
        ChangeCollider();

        if (_lifes < _levels.Count - 1)
            _coroutine = StartCoroutine(RestoreSize());

    }

    #endregion

    /// <summary>
    /// Activa la corrutina del tamaño
    /// </summary>
    private void ActivateCoroutine()
    {
        _coroutine = StartCoroutine(RestoreSize());
    }

    /// <summary>
    /// Modifica las dimensiones del collider según la dirección
    /// </summary>
    private void ChangeCollider()
    {
        switch (_direction)
        {
            case Direction.Left:
                _collider.size = new Vector2(_lifes + 1, 1f);
                _collider.offset =
                    new Vector2(
                        -0.5f * _lifes,
                        0f
                        );
                break;
            case Direction.Right:
                _collider.size = new Vector2(_lifes + 1, 1f);
                _collider.offset =
                    new Vector2(
                        0.5f * _lifes,
                        0f
                        );
                break;
            case Direction.Up:
                _collider.size = new Vector2(1f, _lifes + 1);
                _collider.offset = new Vector2(
                    0f,
                    0.5f * _lifes
                    );
                break;
            case Direction.Down:
                _collider.size = new Vector2(1f, _lifes + 1);
                _collider.offset =
                    new Vector2(
                        0f,
                        -0.5f * _lifes
                        );
                break;
        }
    }

    #endregion


}


