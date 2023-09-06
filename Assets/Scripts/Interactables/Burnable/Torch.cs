using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Utils;

public class Torch : MonoBehaviour, IBurnable, IInteractable
{
    #region SerializeFields

    [SerializeField]
    [Tooltip("Llama de la antorcha")]
    private GameObject _fire;

    [SerializeField] private bool _canAutoDeactivate;
    [SerializeField] private float _autodeactivationSeconds;
    [Tooltip("Introducir los gameObjects tipo Torch que al estar encendidos deja de hacer efecto el autodeactivate")]
    [SerializeField] private Torch[] _stopAutoDeactivateConditions; 

    [Header("Events on fire on/off effects")]
    public UnityEvent OnFireActivation;
    public UnityEvent OnFireOff;

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

    public void Burn(int damage)
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

        if ( _canAutoDeactivate && CanAutoDeactivate() )
            StartCoroutine( AutoDeactivation() );

        OnFireActivation?.Invoke();
    }

    /// <summary>
    /// Desactiva la antorcha
    /// </summary>
    public void DeactivateTorch()
    {
        // Cambiamos el sprite a apagado
        _fire.SetActive(false);
        // E indicamos que se ha desactivado
        _activated = false;

        OnFireOff?.Invoke();
    }

    #endregion

    private IEnumerator AutoDeactivation()
    {
        var wait = new WaitForSeconds(_autodeactivationSeconds);
        yield return wait;
        DeactivateTorch();
    }

    private bool CanAutoDeactivate()
    {
        foreach ( var torch in _stopAutoDeactivateConditions )
            if ( !torch.Activated ) 
                return true;
        return false;
    }
}
