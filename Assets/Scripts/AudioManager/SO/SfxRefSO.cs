// ************ @autor: Álvaro Repiso Romero *************
using UnityEngine;
using FMODUnity;

[CreateAssetMenu(fileName = "SfxRefSO", menuName = "Audio/Sfx Ref SO")]
public class SfxRefSO : ScriptableObject
{
    public EventReference[] Sounds;
}
