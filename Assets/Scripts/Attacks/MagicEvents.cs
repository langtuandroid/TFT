using Attack;
using System;
using System.Collections;
using UnityEngine;

public class MagicEvents : MonoBehaviour
{

    #region Events

    // Eventos para recarga de los poderes máximos
    public event Action<MaxPowerValues> OnMaxPowerValueChange;

    #endregion

    #region Public methods

    public void SetPanelColor(IAttack attack)
    {
        PowerPanelsManager.Instance.ChangePanelColor(attack);
    }

    #region Coroutines

    public void MaxPowerActivated(IAttack attack)
    {
        StartCoroutine(IncrementFillAmount(attack));
    }

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

    /// <summary>
    /// Bucle que va incrementando un fillAmount
    /// de un evento
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    private IEnumerator IncrementFillAmount(IAttack attack)
    {
        for (float i = 0f; i < 1f; i += 0.001f)
        {
            ChangeFillAmount(new MaxPowerValues(i, attack));
            yield return new WaitForSeconds(0.005f);
        }

        // Finalmente, se pone a 1 exacto
        ChangeFillAmount(new MaxPowerValues(1f, attack));
    }

    #endregion


}
