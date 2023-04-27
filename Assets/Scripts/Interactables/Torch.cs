using UnityEngine;
using Utils;

public class Torch : MonoBehaviour
{
    #region SerializeFields

    [SerializeField]
    private Sprite _on, _off; // Sprites para cuando est� encendido y/o apagado
    #endregion

    #region Private Variables
    private SpriteRenderer _spriteRend; // SpriteRenderer del objeto
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
        // Obtenemos componentes
        _spriteRend = GetComponent<SpriteRenderer>();

        // Inicializamos variables
        _activated = false; // Por defecto no ha sido activado
        _spriteRend.sprite = _off; // Y el sprite es de apagado
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si no ha sido activado previamente
        // y colisiona con la bola de fuego
        if (!_activated &&
            collision.gameObject.CompareTag(Constants.TAG_FIRE_BALL))
        {
            // Destruimos la bola de fuego
            Destroy(collision.gameObject);
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
        _spriteRend.sprite = _on;
        // E indicamos que se ha activado
        _activated = true;
    }

    public void DeactivateTorch()
    {
        // Cambiamos el sprite a apagado
        _spriteRend.sprite = _off;
        // E indicamos que se ha desactivado
        _activated = false;
    }

    #endregion


}
