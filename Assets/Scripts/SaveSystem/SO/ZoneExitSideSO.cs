using UnityEngine;

[CreateAssetMenu( fileName = "ZoneExitSideSO" , menuName = "SaveData/ZoneExitSideSO" )]
public class ZoneExitSideSO : ScriptableObject
{
    [Tooltip("Only positive values")]
    public int nextStartPointRefID;
}