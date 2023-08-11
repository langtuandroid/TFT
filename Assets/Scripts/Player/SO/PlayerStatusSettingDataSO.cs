using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatusSettingDataSO", menuName = "Player/PlayerStatusSettingDataSO")]
public class PlayerStatusSettingDataSO : ScriptableObject
{
    [Header("Magic Attack Settings")]
    [Tooltip("Tiempo entre ataques mágicos")]
    public float TimeBetweenMagicAttacks = .2f;
    [Tooltip("Tiempo que tarda en recuperar magia")]
    public float TimeOfRecovering = .8f;
    [Tooltip("Tiempo de recarga del poder máximo")]
    public float TimeToRechargeMaxPower = 30f;
    [Tooltip("Duraci�n del poder m�ximo en pantalla")]
    public float MaxPowerDuration = 10f;

    [Header("Life & Health settings")]
    [Tooltip("Tiempo que dura la invencibilidad tras recibir da�o")]
    public float TimeOfInvencibility = 1.5f;

    [Header("Stunning")]
    [Tooltip("Tiempo que pasa el jugador aturdido")]
    public float TimeStunned = 5f;
}
