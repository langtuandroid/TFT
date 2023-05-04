// ************ @autor: Álvaro Repiso Romero *************
using UnityEngine;
using FMODUnity;

namespace Audio
{
    [CreateAssetMenu( fileName = "SfxRefSO" , menuName = "Audio/Sfx Ref SO" )]
    public class SfxRefSO : ScriptableObject
    {
        public EventReference[] Sounds;
    }
}