// ************ @autor: Álvaro Repiso Romero *************
using UnityEngine;

[CreateAssetMenu(fileName = "ZoneSaveSO", menuName = "SaveData/ZoneSaveSO" )]
public class ZoneSaveSO : ScriptableObject
{
    public ZoneSave zoneSave;

    public void NewGameReset()
    {
        zoneSave = new ZoneSave();
    }
}
