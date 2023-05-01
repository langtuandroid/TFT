using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEditor.Tilemaps
{
#if UNITY_EDITOR
    [CustomGridBrush( true , false , false , "Checker Brush" )]
    public class CheckerBrush : GridBrushBase
    {
        public TileBase tileA;
        public TileBase tileB;

        public override void Paint( GridLayout grid , GameObject brushTarget , Vector3Int position )
        {
            Tilemap tilemap = brushTarget.GetComponent<Tilemap>();
            if ( tilemap != null )
            {
                SetCheckeredTile( tilemap , position );
            }
        }

        private void SetCheckeredTile( Tilemap tilemap , Vector3Int position )
        {
            bool evenX = position.x % 2 == 0;
            bool evenY = position.y % 2 == 0;
            TileBase choice;
            if ( evenX && evenY ) choice = tileA;
            else if ( evenX && !evenY ) choice = tileB;
            else if ( !evenX && evenY ) choice = tileB;
            else choice = tileA;
            Undo.RegisterCompleteObjectUndo( tilemap , "Checker Tile Placement" );
            tilemap.SetTile( position , choice );
        }
        // there are many other methods you can override
        // to get an idea how to implement, see the examples brushes in that github link
    }

    [CustomEditor( typeof( CheckerBrush ) )]
    public class CheckerBrushEditor : GridBrushEditorBase
    {
        /// <summary>Returns all valid targets that the brush can edit.</summary>
        /// <remarks>Valid targets for the CheckeredBrush are any GameObjects with a Tilemap component.</remarks>
        public override GameObject[] validTargets
        {
            get
            {
                return GameObject.FindObjectsOfType<Tilemap>().Select( x => x.gameObject ).ToArray();
            }
        }
    }
#endif
}

