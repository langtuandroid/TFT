using UnityEngine;

public abstract class SecondaryAction : MonoBehaviour
{
    #region Internal variables

    // STATES
    internal bool _isUsingEffect;

    #endregion

    #region Public variables

    // EVENTS
    public bool IsUsingEffect => _isUsingEffect;

    #endregion

    #region Protected variables

    protected Vector2 direction;

    #endregion

    #region Unity methods

    private void Awake()
    {
        _isUsingEffect = false;
    }

    #endregion

    #region Abstract class methods

    public virtual void Select()
    {
        // TODO: Activar evento para cambiar la UI
    }

    /// <summary>
    /// Define el comportamiento de la acción secundaria
    /// </summary>
    public virtual void Effect() { }

    /// <summary>
    /// Detiene el efecto de la acción secundaria
    /// </summary>
    public virtual void StopEffect() { }

    /// <summary>
    /// Define la acción hacia la que se dirige la acción secundaria
    /// </summary>
    /// <param name="direction"></param>
    public virtual void SetDirection(Vector2 direction)
    {
        this.direction = direction;
    }

    #endregion

}
