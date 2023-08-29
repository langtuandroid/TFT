using UnityEngine;

[CreateAssetMenu(fileName = "FireAttackSettingsSO", menuName = "Magic/FireAttackSettingsSO")]
public class FireAttackSettingsSO : MagicAttackSettingsSO
{
    [Header("Weak Attack")]
    [Tooltip("Weak attack prefab")]
    public GameObject WeakPrefab;

    [Header("Medium Attack")]
    [SerializeField]
    [Tooltip("Medium attack prefab")]
    public GameObject _mediumPrefab;
    [Tooltip("Tiempo que debe pasar para que el lanzallamas consuma")]
    public float TimeBetweenConsuming = .4f;

    [Header("Strong Attack")]
    [Tooltip("Strong attack prefab")]
    public GameObject StrongPrefab;
    [Tooltip("Cantidad de da�o que produce el poder m�ximo")]
    public int StrongAttackDamage = 10;
}
