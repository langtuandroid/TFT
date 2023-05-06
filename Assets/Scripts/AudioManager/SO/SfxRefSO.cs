// ************ @autor: Álvaro Repiso Romero *************
using UnityEngine;
using FMODUnity;

namespace Audio
{
    [CreateAssetMenu( fileName = "SfxRefSO" , menuName = "Audio/Sfx Ref SO" )]
    public class SfxRefSO : ScriptableObject
    {
        [System.Serializable]
        public class Sfx
        {
            public string SoundName;
            public int Id;
            public EventReference Sound;
        }

        public EventReference[] Sounds;
        public Sfx[] SfxRef;
    }
}