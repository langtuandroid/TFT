using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEditor.Tilemaps
{
    [CustomGridBrush( true , false , false , "Each Two Brush" )]
    public class EachTwoBrush : GridBrushBase
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
            else choice = tileB;
            Undo.RegisterCompleteObjectUndo( tilemap , "Checker Tile Placement" );
            tilemap.SetTile( position , choice );
        }
        // there are many other methods you can override
        // to get an idea how to implement, see the examples brushes in that github link
    }

    [CustomEditor( typeof( EachTwoBrush ) )]
    public class EachTwoBrushEditor : GridBrushEditorBase
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
}