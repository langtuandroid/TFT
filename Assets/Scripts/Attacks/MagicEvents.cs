using Attack;
using System;

public class MagicEvents
{
    #region Events

    // Eventos para recarga de los poderes máximos
    public event Action<MagicAttack> OnAttackTypeValue;
    public event Action<float> OnFillAmountValue;
    public event Action<float> OnPanelAlphaValue;
    public event Action<float> OnMaxPowerUsedValue;
    public event Action OnMaxPowerFinalizedValue;

    #endregion

    #region Public variables

    // Tiempo que tarda en recargar el poder máximo
    public float MaxPowerRechargingTime;

    #endregion

    #region Public methods

    #region Events methods

    public void ChangePanelAlphaAmount(float alpha)
    {
        OnPanelAlphaValue?.Invoke(alpha);
    }

    public void ChangeAttackType(MagicAttack attack)
    {
        OnAttackTypeValue?.Invoke(attack);
    }

    public void ChangeFillAmount(float fillAmount)
    {
        OnFillAmountValue?.Invoke(fillAmount);
    }

    public void MaxPowerUsed(float time)
    {
        OnMaxPowerUsedValue?.Invoke(time);
    }

    public void MaxPowerFinalized()
    {
        OnMaxPowerFinalizedValue?.Invoke();
    }

    #endregion

    #region Variable definitions

    public void DefineMaxPowerRechargingTime(float maxPowerRechargingTime)
    {
        MaxPowerRechargingTime = maxPowerRechargingTime;
    }

    #endregion

    #endregion


}
