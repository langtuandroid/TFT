using System;

public class LifeEvents
{
    // Eventos para las vidas
    public event Action OnHeartsValue; // Cantidad de corazones
    public event Action<int> OnCurrentLifeValue; // Cantidad de vida

    public void AddHeart()
    {
        OnHeartsValue.Invoke();
    }

    public void ChangeCurrentLifeQuantity(int quantity)
    {
        OnCurrentLifeValue.Invoke(quantity);
    }

}
