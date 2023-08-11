using UnityEngine;

[CreateAssetMenu(fileName = "DungeonDataSO", menuName = "Level Control/Dungeon Data")]
public class DungeonDataSO : ScriptableObject
{
    [Header("Dungeon Size")]
    [Range(3, 15)] public int DungeonGridSize = 8;
    [Range(3, 15)] public int NumOfRoomsPerPath = 10;
    [Range(2, 15)] public int NumOfPaths = 3;
}
