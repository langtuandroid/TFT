using Attack;
using System;
using System.Collections;
using UnityEngine;

public class MagicEvents
{
    #region Events

    // Eventos para recarga de los poderes máximos
    public event Action<MagicAttack> OnAttackTypeValue;
    public event Action<float> OnFillAmountValue;
    public event Action<float> OnPanelAlphaValue;

    #endregion

    #region Public methods

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

    #endregion


}
