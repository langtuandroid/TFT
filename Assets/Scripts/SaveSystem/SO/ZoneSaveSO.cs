using UnityEngine;
using SaveSystem;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ZoneSaveSO", menuName = "SaveData/ZoneSaveSO" )]
public class ZoneSaveSO : ScriptableObject
{
    public ZoneSave zoneSave;

    public void NewGameReset()
    {
        zoneSave.IsCompleted = false;
        zoneSave.IsActivatedList = new List<bool>();
    }
}
