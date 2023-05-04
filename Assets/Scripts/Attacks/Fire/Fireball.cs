using UnityEngine;

public class Fireball : MonoBehaviour
{
    #region SerializeFields
    [SerializeField]
    private float _speed; // Velocidad de movimiento
    [SerializeField]
    private float _lifeTime; // Tiempo de vida del objeto
    #endregion

    #region Private Variables
    private Vector3 _direction; // Dirección de movimiento
    private float _timer; // Temporizador
    #endregion


    #region Unity Methods

    private void Awake()
    {
        // Ponemos una dirección de movimiento por defecto
        // (p. ej. hacia arriba)
        _direction = Vector3.up;
    }

    private void Update()
    {
        // Movemos el objeto
        Move();
        // E incrementamos su tiempo
        IncrementTime();
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
