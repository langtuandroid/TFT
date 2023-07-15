using UnityEngine;

public class Fireball : MonoBehaviour
{
    #region SerializeFields
    [SerializeField]
    [Tooltip("Velocidad de movimiento")]
    private float _speed;
    [SerializeField]
    [Tooltip("Tiempo de vida del objeto")]
    private float _lifeTime;
    [SerializeField]
    [Tooltip("Daño que hace al impactar")]
    private int _damage = 1;
    #endregion

    #region Private Variables
    private IAudioSpeaker _audioSpeaker;
    private GameInputs _gameInputs;
    private Vector3 _direction; // Dirección de movimiento
    private float _timer; // Temporizador
    #endregion


    #region Unity Methods

    private void Awake()
    {
        // Ponemos una dirección de movimiento por defecto
        // (p. ej. hacia arriba)
        _direction = Vector3.up;
        _audioSpeaker = ServiceLocator.GetService<IAudioSpeaker>();
        _gameInputs = ServiceLocator.GetService<GameInputs>();
    }

    private void Update()
    {
        // Movemos el objeto
        Move();
        // E incrementamos su tiempo
        IncrementTime();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<IBurnable>()?.Burn(_damage);
        _audioSpeaker.PlaySound(AudioID.G_FIRE, AudioID.SS_FIRE_BALL_HIT, transform.position);
        _gameInputs.RumblePad(0.2f, 0.5f, 0.2f);

        DisappearBall();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Mueve la bola de fuego
    /// </summary>
    private void Move()
    {
        // Vamos incrementando la posición en la dirección dada
        transform.position += _speed * Time.deltaTime * _direction;
    }

    /// <summary>
    /// Incrementa el temporizador de la bola
    /// y si lleva un tiempo mayor que el suyo de vida se destruye
    /// </summary>
    private void IncrementTime()
    {
        // Incrementamos el contador
        _timer += Time.deltaTime;
        // Si supera al tiempo de vida
        if (_timer >= _lifeTime)
            // Destruimos la bola de fuego
            Destroy(gameObject);
    }

    private void DisappearBall()
    {
        // TODO: Añadir animación de la bola chocando
        Destroy(gameObject);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Establece la dirección de movimiento de la bola de fuego
    /// </summary>
    /// <param name="direction"></param>
    public void SetDirection(Vector3 direction)
    {
        // Establecemos la dirección de movimiento con la nueva dirección
        _direction = direction;
    }

    #endregion


}
