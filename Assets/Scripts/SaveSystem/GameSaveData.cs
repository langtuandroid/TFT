// ************ @autor: Álvaro Repiso Romero *************
using System;

[Serializable]
public class GameSaveData
{
    public int startSavePoint;
    public int startPointRefID;

    public PlayerStatusSave playerStatusSave;

    public ZoneSaveSO[] zoneSaveSOs;
}
