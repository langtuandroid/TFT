using UnityEngine;


[CreateAssetMenu(fileName = "PlantAttackSettingsSO", menuName = "Magic/PlantAttackSettingsSO")]
public class PlantAttackSettingsSO : MagicAttackSettingsSO
{
    [Header("Weak Attack")]
    [Tooltip("Tiempo para que se cure")]
    public float TimeToHeal = .8f;

    [Header("Medium Attack")]
    [Tooltip("Medium attack prefab")]
    public GameObject MediumPrefab;

}
