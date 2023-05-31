// ************ @autor: Álvaro Repiso Romero *************
using UnityEngine;
using FMODUnity;
using System;
using System.Collections.Generic;

namespace Audio
{
    //[CreateAssetMenu( fileName = "GameMusicSO" , menuName = "Audio/GameMusicSO" )]
    public class GameMusicSO : ScriptableObject
    {
        [Serializable]
        public struct GameMusic
        {
            public string name;
            public MusicName musicName;
            public EventReference musicReference;
        }

        public List<GameMusic> gameMusicList;

        public Dictionary<MusicName, EventReference> GameMusicDictionary()
        {
            Dictionary<MusicName, EventReference> musicDict = new();

            foreach (GameMusic music in gameMusicList )
                musicDict.Add( music.musicName , music.musicReference );

            return musicDict;
        }
    }
}