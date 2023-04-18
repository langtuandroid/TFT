using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu( fileName = "HighGround RuleTile", menuName = "2D/Tiles/High Ground Rule Tile" )]
public class HighGroundRuleTile : RuleTile<HighGroundRuleTile.Neighbor> {
    public TileBase caveRuleTile;
    public TileBase fifthRuleTile;

    public class Neighbor : RuleTile.TilingRule.Neighbor
    {
        public const int Nothing = 3;
        public const int Any = 4;
        public const int FifthRuleTile = 5;
    }

    public override bool RuleMatch( int neighbor , TileBase tile )
    {
        switch ( neighbor )
        {
            case Neighbor.This:    return tile == this 
                                       || tile == caveRuleTile;
            case Neighbor.Nothing: return tile == null;
            case Neighbor.Any:     return tile != null;
            case Neighbor.FifthRuleTile: return fifthRuleTile == tile
                                             || caveRuleTile == tile;
        }
        return base.RuleMatch( neighbor , tile );
    }
}