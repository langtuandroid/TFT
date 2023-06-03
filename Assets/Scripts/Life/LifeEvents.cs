using System;

public class LifeEvents
{
    #region Events

    // Eventos para las vidas
    public event Action OnHeartsValue; // Para actualizar la cantidad de corazones
    public event Action<int> OnCurrentLifeValue; // Cantidad de vida
    public event Action OnDeathValue; // Para activar la animación de muerte

    #endregion

    #region Public variables

    public bool IsDeath { get; private set; }

    #endregion

    #region Public methods

    public void AddHeart()
    {
        OnHeartsValue.Invoke();
    }

    public void ChangeCurrentLifeQuantity(int quantity)
    {
        OnCurrentLifeValue.Invoke(quantity);
    }

    public void OnDeath()
    {
        OnDeathValue.Invoke();
        IsDeath = true;
    }

    #endregion

}
