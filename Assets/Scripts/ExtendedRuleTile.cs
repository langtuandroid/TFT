using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu( fileName = "New Extended Rule Tile", menuName = "2D/Tiles/Extended Rule Tile")]
public class ExtendedRuleTile : RuleTile<ExtendedRuleTile.Neighbor> {
    public List<TileBase> caveTiles;

    public class Neighbor : RuleTile.TilingRule.Neighbor {
        public const int Any = 3;
        public const int Specified = 4;
        public const int Nothing = 5;
    }

    public override bool RuleMatch(int neighbor, TileBase tile) {        
        switch ( neighbor )
        {
            case Neighbor.Any:       return tile != null;
            case Neighbor.Specified: return CheckSpecified( tile , neighbor );
            case Neighbor.Nothing:   return tile == null;
        }
        return base.RuleMatch(neighbor, tile);
    }


    private int GetIdFromTile( TileBase tile ) 
    {
        foreach ( var tilingRule in m_TilingRules )
            if ( tilingRule.Equals( tile ) )
                return tilingRule.m_Id;
        return 0;
    }


    private bool CheckSpecified( TileBase tile , int neighbor )
    {
        switch ( GetIdFromTile( tile ) )
        {
            case 0: return CheckTile0( neighbor );
            case 1: return false;
            case 2: return false;
        }

        Debug.Log( "Aquí no debería llegar, avisar a Alvaro" );
        return true;
    }


    private bool CheckTile0( int neighbor )
    {
        TilingRule rule = m_TilingRules[ 0 ];
        switch ( neighbor )
        {
            case 1: return true; //  0, 1
            case 3: return false; // -1, 0
            case 4: return false; //  1, 0
        }
        return true;
    }
    
    private bool CheckTile( int neighbor )
    {
        TilingRule rule = m_TilingRules[ 0 ];
        switch ( neighbor )
        {
            case 0: return true;  // -1, 1
            case 1: return false; //  0, 1
            case 2: return false; //  1, 1
            case 3: return false; // -1, 0
            case 4: return false; //  1, 0
            case 5: return false; // -1,-1
            case 6: return false; //  0,-1
            case 7: return false; //  1,-1
        }
        return true;
    }
}