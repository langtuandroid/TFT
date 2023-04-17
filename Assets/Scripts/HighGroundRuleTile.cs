using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu( fileName = "HighGround RuleTile", menuName = "2D/Tiles/High Ground Rule Tile" )]
public class HighGroundRuleTile : RuleTile<HighGroundRuleTile.Neighbor> {
    public TileBase caveRuleTile;
    public TileBase fifthRuleTile;
    public TileBase sixthRuleTile;

    public class Neighbor : RuleTile.TilingRule.Neighbor
    {
        public const int Nothing = 3;
        public const int Any = 4;
        public const int FifthRuleTile = 5;
        public const int SixthRuleTile = 6;
        public const int FifthAndSixthRuleTile = 7;
    }

    public override bool RuleMatch( int neighbor , TileBase tile )
    {
        switch ( neighbor )
        {
            case Neighbor.This: return tile == this || tile == caveRuleTile;
            case Neighbor.Nothing: return tile == null;
            case Neighbor.Any: return tile != null;
            case Neighbor.FifthRuleTile: return IsFifthRuleTile( tile );
            case Neighbor.SixthRuleTile: return IsSixthRuleTile( tile );
            case Neighbor.FifthAndSixthRuleTile: return IsFifthRuleTile( tile ) 
                                                     || IsSixthRuleTile( tile );
        }
        return base.RuleMatch( neighbor , tile );
    }

    private bool IsSixthRuleTile( TileBase tile )
    {
        return sixthRuleTile == tile
            || caveRuleTile == tile;
    }

    private bool IsFifthRuleTile( TileBase tile )
    {
        return fifthRuleTile == tile
            || caveRuleTile == tile;
    }
}