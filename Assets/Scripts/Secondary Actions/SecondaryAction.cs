using UnityEngine;

public abstract class SecondaryAction : MonoBehaviour
{
    protected Vector2 direction;

    public virtual void Select()
    {
        // TODO: Activar evento para cambiar la UI
    }

    /// <summary>
    /// Define el comportamiento de la acción secundaria
    /// </summary>
    public virtual void Effect() { }

    /// <summary>
    /// Define la acción hacia la que se dirige la acción secundaria
    /// </summary>
    /// <param name="direction"></param>
    public virtual void SetDirection(Vector2 direction)
    {
        this.direction = direction;
    }

}
