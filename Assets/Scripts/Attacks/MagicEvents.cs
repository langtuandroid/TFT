using Attack;
using System;
using System.Collections;
using UnityEngine;

public class MagicEvents
{

    #region Events

    // Eventos para recarga de los poderes máximos
    public event Action<MaxPowerValues> OnMaxPowerValueChange;

    #endregion

    #region Public methods

    public void SetPanelColor(IAttack attack)
    {
        PowerPanelsManager.Instance.ChangePanelColor(attack);
        PowerPanelsManager.Instance.SetAlpha(0f);
    }

    #region Coroutines

    public void ChangeMaxPowerValue(float value, IAttack attack)
    {
        MaxPowerValues maxPower = new MaxPowerValues(value, attack);
        ChangeFillAmount(maxPower);
    }

    #endregion

    #endregion

    #region Private methods

    /// <summary>
    /// Cambia el valor flotante de un evento
    /// </summary>
    /// <param name="action"></param>
    /// <param name="value"></param>
    private void ChangeFillAmount(MaxPowerValues maxPower)
    {
        OnMaxPowerValueChange?.Invoke(maxPower);
    }



    #endregion


}
