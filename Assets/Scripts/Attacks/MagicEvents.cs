using Attack;
using System;
using System.Collections;
using UnityEngine;

public class MagicEvents : MonoBehaviour
{

    #region Events

    // Eventos para recarga de los poderes máximos
    public event Action<float> OnFireValueChange; // Fuego
    public event Action<float> OnLeafValueChange; // Planta
    public event Action<float> OnWaterValueChange; // Agua

    #endregion

    #region Public methods

    #region Coroutines

    /// <summary>
    /// Activa la corrutina del poder de fuego
    /// </summary>
    public void FirePowerActivated()
    {
        StartCoroutine(IncrementFillAmount(OnFireValueChange));
    }

    public void ChangePowerValue(float value, IAttack attack)
    {
        var type = attack.GetType();

        // Si tenemos el poder de fuego
        if (type == typeof(FireAttack))
            ChangeFillAmount(OnFireValueChange, value);
    }

    #endregion
    #endregion

    #region Private methods

    /// <summary>
    /// Cambia el valor flotante de un evento
    /// </summary>
    /// <param name="action"></param>
    /// <param name="value"></param>
    private void ChangeFillAmount(Action<float> action, float value)
    {
        action?.Invoke(value);
    }

    /// <summary>
    /// Bucle que va incrementando un fillAmount
    /// de un evento
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    private IEnumerator IncrementFillAmount(Action<float> action)
    {
        for (float i = 0f; i < 1f; i += 0.001f)
        {
            ChangeFillAmount(action, i);
            yield return new WaitForSeconds(0.005f);
        }

        // Finalmente, se pone a 1 exacto
        action?.Invoke(1f);
    }

    #endregion


}
