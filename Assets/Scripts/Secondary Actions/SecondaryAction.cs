using UnityEngine;

public abstract class SecondaryAction : MonoBehaviour
{
    protected Vector2 direction;

    public virtual void Select()
    {
        // TODO: Activar evento para cambiar la UI
    }

    /// <summary>
    /// Define el comportamiento de la acci�n secundaria
    /// </summary>
    public virtual void Effect() { }

    /// <summary>
    /// Define la acci�n hacia la que se dirige la acci�n secundaria
    /// </summary>
    /// <param name="direction"></param>
    public virtual void SetDirection(Vector2 direction)
    {
        this.direction = direction;
    }

}
