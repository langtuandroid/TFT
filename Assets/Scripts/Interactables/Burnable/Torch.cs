using UnityEngine;
using Utils;

public class Torch : MonoBehaviour, IBurnable, IInteractable
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

    #endregion

    #region Public Methods

    public void Burn()
    {
        // Si se quema, activamos la antorcha
        ActivateTorch();
    }

    public void Interact(Vector2 lookDirection)
    {
        Debug.Log("Maybe I can activate it with fire");
    }

    public void ShowCanInteract(bool show)
    {
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Activa la antorcha
    /// </summary>
    private void ActivateTorch()
    {
        if (_activated)
            return;

        // Cambiamos el sprite a encendido
        _fire.SetActive(true);
        // E indicamos que se ha activado
        _activated = true;
    }

    /// <summary>
    /// Desactiva la antorcha
    /// </summary>
    private void DeactivateTorch()
    {
        // Cambiamos el sprite a apagado
        _fire.SetActive(false);
        // E indicamos que se ha desactivado
        _activated = false;
    }

    #endregion



}
