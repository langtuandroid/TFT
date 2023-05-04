using UnityEngine;
using Utils;

public class Torch : MonoBehaviour
{
    #region SerializeFields

    [SerializeField]
    [Tooltip("Llama de la antorcha")]
    private GameObject _fire;

    #endregion

    #region Private Variables
    private bool _activated; // Booleano para ver si ha sido o no activado
    public bool Activated
    {
        get { return _activated; }
        set { _activated = value; }
    }
    #endregion

    #region Unity Methods

    private void Awake()
    {
        // Inicializamos variables
        _activated = false; // Por defecto no ha sido activado
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si no ha sido activado previamente
        // y colisiona con la bola de fuego
        if (collision.gameObject.CompareTag(Constants.TAG_FIRE_BALL))
        {
            // Destruimos la bola de fuego
            Destroy(collision.gameObject);

            if (!_activated)
                ActivateTorch();
        }
        else if (!_activated &&
            collision.gameObject.CompareTag(Constants.TAG_FLAMES))
            // Activamos la antorcha
            ActivateTorch();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Activa la antorcha
    /// </summary>
    public void ActivateTorch()
    {
        // Cambiamos el sprite a encendido
        _fire.SetActive(true);
        // E indicamos que se ha activado
        _activated = true;
    }

    public void DeactivateTorch()
    {
        // Cambiamos el sprite a apagado
        _fire.SetActive(false);
        // E indicamos que se ha desactivado
        _activated = false;
    }

    #endregion


}
