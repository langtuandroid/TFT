using System;

public class LifeEvents
{
    #region Events

    // Eventos para las vidas
    public event Action OnHeartsValue; // Para actualizar la cantidad de corazones
    public event Action<int> OnCurrentLifeValue; // Cantidad de vida
    public event Action OnDeathValue; // Para activar la animación de muerte
    public event Action OnFallDown; // Para activar la animación de muerte
    public event Action<float> OnStartTemporalInvencibility; // Para activar invencibilidad temporal
    public event Action OnStopTemporalInvencibility; // Para desactivar invencibilidad temporal

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
    }

    public void FallDown()
    {
        OnFallDown?.Invoke();
    }

    public void StartTemporalInvencibility(float time)
    {
        OnStartTemporalInvencibility?.Invoke(time);
    }

    public void StopTemporalInvencibility()
    {
        OnStopTemporalInvencibility?.Invoke();
    }

    #endregion

}
