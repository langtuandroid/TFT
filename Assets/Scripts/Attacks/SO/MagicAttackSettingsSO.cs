using UnityEngine;

[CreateAssetMenu(fileName = "MagicAttackSettingsSO", menuName = "Magic/MagicAttackSettingsSO")]
public class MagicAttackSettingsSO : ScriptableObject
{
    [Header("Attack costs")]
    [SerializeField]
    [Tooltip("Coste de los ataques (de menor a mayor poder)")]
    public int[] Costs = new int[3];

}
