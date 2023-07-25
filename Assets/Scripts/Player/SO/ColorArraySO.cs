using UnityEngine;

[CreateAssetMenu(fileName = "ColorArraySO", menuName = "Player/Color Array SO")]
public class ColorArraySO :ScriptableObject
{
    [field: SerializeField] public Color[] ColorArray { get; private set; }
}
