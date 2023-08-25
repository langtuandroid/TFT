using DG.Tweening;
using Player;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BallThrower : MonoBehaviour, IBurnable, IPunchable
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

    [Header("Life")]
    [SerializeField]
    [Tooltip("Vida del enemigo")]
    private int _life = 6;

    [SerializeField]
    [Tooltip("Lista de almas que deja al morir")]
    private List<GameObject> _souls;

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

    // SERVICES
    private GameStatus _gameStatus;

    // COMPONENTS
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRend;

    // VARIABLES
    private Vector2 _direction;
    private FSMBallThrower _stateBallThrower;
    private Tween _tween;
    private bool _die;
    private GameStatus.GameState _gameState;

    #endregion

    #region Unity Methods

    private void Start()
    {
        // Inicializamos
        Init();
    }

    private void Update()
    {
        // TODO: Preguntar el estado
        if (_gameState != GameStatus.GameState.GamePlay || _die)
            return;
        // Ejecutamos el estado
        _stateBallThrower.Execute(_direction);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<PlayerStatus>()?.TakeDamage(1);
    }

    private void OnDestroy()
    {
        _gameStatus.OnGameStateChanged += OnGameStateChangedValue;
    }

    #endregion

    #region Public methods

    #region Damage

    public void Punch(int damage)
    {
        GetDamage(damage);
    }

    public void Burn(int damage)
    {
        GetDamage(damage);
    }

    #endregion


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
        // SERVICES
        _gameStatus = ServiceLocator.GetService<GameStatus>();
        _gameStatus.OnGameStateChanged += OnGameStateChangedValue;

        // COMPONENTS
        _rb = GetComponent<Rigidbody2D>();
        _spriteRend = GetComponent<SpriteRenderer>();

        // VARIABLES
        _die = false;
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
        _gameState = GameStatus.GameState.GamePlay;
    }

    private void GetDamage(int damage)
    {
        _life -= damage;

        if (_life <= 0)
            Die();
        else
        {
            if (_tween != null)
                _tween.Kill();

            _tween = DamageAnimation();
        }
    }


    private void Die()
    {
        Sequence seq = DOTween.Sequence();
        // We kill the tween and put the normal color
        if (_tween != null)
        {
            _tween.Kill();
            seq.Append(_spriteRend.DOColor(Color.white, 0f));
        }
        _die = true;

        // TODO: Activate die animation

        seq.AppendCallback(() =>
        {
            // Giving the souls
            foreach (GameObject soul in _souls)
            {
                soul.transform.parent = null;
                soul.SetActive(true);
            }
        });
        // We deactivate collider
        seq.AppendCallback(() => this.GetComponent<Collider2D>().enabled = false);
        seq.Append(_spriteRend.DOFade(0f, 2f));
        // And finally, we deactivate the gameObject
        seq.OnComplete(() => Destroy(gameObject));

        seq.Play();
    }

    private Tween DamageAnimation()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_spriteRend.DOColor(Color.red, 0f));
        seq.Append(_spriteRend.DOColor(Color.white, .5f));
        seq.Play();

        return seq;
    }

    private void OnGameStateChangedValue(GameStatus.GameState state)
    {
        _gameState = state;
    }

    #endregion


}
