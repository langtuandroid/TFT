// ************ @autor: Álvaro Repiso Romero *************
using UnityEngine;

//[CreateAssetMenu( fileName = "ZoneExitSideSO" , menuName = "Level Control/ZoneExitSideSO" )]
public class ZoneExitSideSO : ScriptableObject
{
    [Tooltip("Only positive values")]
    public int nextStartPointRefID;

    public void NewGameReset() => nextStartPointRefID = 0;
}