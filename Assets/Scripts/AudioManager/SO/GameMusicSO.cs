// ************ @autor: Álvaro Repiso Romero *************
using UnityEngine;
using FMODUnity;

namespace Audio
{
    //[CreateAssetMenu( fileName = "GameMusicSO" , menuName = "Audio/GameMusicSO" )]
    public class GameMusicSO : ScriptableObject
    {
        public EventReference MainMenu;
        public EventReference Woods;
        public EventReference WoodsDungeon;
        public EventReference Volcan;
        public EventReference VolcanDungeon;
        public EventReference WaterRuin;
        public EventReference WaterRuinDungeon;
    }
}