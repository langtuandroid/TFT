// ************ @autor: Álvaro Repiso Romero *************
using UnityEngine;
using FMODUnity;

namespace Audio
{
    //[CreateAssetMenu( fileName = "GameMusicSO" , menuName = "Audio/GameMusicSO" )]
    public class GameMusicSO : ScriptableObject
    {
        public EventReference[] gameMusic;
    }
}