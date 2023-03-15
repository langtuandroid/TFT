// ************ @autor: Álvaro Repiso Romero *************
using UnityEngine;
using UnityEditor;

namespace Procedural
{
    [CustomEditor( typeof( DungeonGenerator ) )]
    public class DungeonGeneratorEditor : Editor
    {
        //public override void OnInspectorGUI()
        //{
        //    base.OnInspectorGUI();
        //    DungeonGenerator dungeonGenerator = (DungeonGenerator)target;
        //    if ( GUILayout.Button( "Clear Dungeon" ) )
        //    {
        //        dungeonGenerator.ClearMap();
        //    }
        //    if ( GUILayout.Button( "Create Dungeon" ) )
        //    {
        //        dungeonGenerator.Init();
        //    }
        //    serializedObject.Update();
        //}
    }
}