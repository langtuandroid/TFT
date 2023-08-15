using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BallThrower : MonoBehaviour
{
    #region Other classes & enums

    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    #endregion


    #region SerializeFields

    [Header("Settings")]
    [SerializeField]
    [Tooltip("Tiempo que pasa caminando")]
    private float _timeWalking;

    [SerializeField]
    [Tooltip("Tiempo que tarda en recargar la bola")]
    private float _timeToRechargeBall;

    [SerializeField]
    [Tooltip("Velocidad de movimiento")]
    private float _velocity;

    [SerializeField]
    [Tooltip("Dirección hacia la que miran en un primer momento")]
    private Direction _lookDirection;

    [Header("Elements")]
    [SerializeField]
    private GameObject _ballPrefab;

    #endregion

    #region Public variables

    // COMPONENTS
    public Rigidbody2D Rb => _rb;

    // ELEMENTS
    public GameObject BallPrefab => _ballPrefab;

    // VARIABLES
    public float TimeWalking => _timeWalking;
    public float TimeToRechargeBall => _timeToRechargeBall;
    public float Velocity => _velocity;

    #endregion

    #region Private variables

    // COMPONENTS
    private Rigidbody2D _rb;

    // VARIABLES
    private Vector2 _direction;
    private FSMBallThrower _stateBallThrower;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        // Inicializamos
        Init();
    }

    private void Update()
    {
        // Ejecutamos el estado
        _stateBallThrower.Execute(_direction);
    }

    #endregion

    #region Public methods

    public void ChangeToOtherState(FSMBallThrower newState)
    {
        _stateBallThrower = newState;
    }

    public void ChangeDirection(Vector2 direction)
    {
        _direction = direction;
    }

    #endregion

    #region Private methods

    private void Init()
    {
        // COMPONENTS
        _rb = GetComponent<Rigidbody2D>();


        // VARIABLES
        switch (_lookDirection)
        {
            case Direction.Up:
                _direction = Vector2.up;
                break;
            case Direction.Down:
                _direction = Vector2.down;
                break;
            case Direction.Left:
                _direction = Vector2.left;
                break;
            case Direction.Right:
                _direction = Vector2.right;
                break;
        }
        _stateBallThrower = new BallThrowerAdvanceState(this);
    }

    #endregion


}
