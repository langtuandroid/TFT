using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class DownRuleTile : RuleTile<DownRuleTile.Neighbor> {
    public TileBase caveRuleTile;
    public TileBase topRuleTile;
    public TileBase middleWallRuleTile;

    public class Neighbor : RuleTile.TilingRule.Neighbor {
        public const int Nothing = 3;
        public const int Any = 4;
        public const int TopRuleTile = 5;
        public const int MiddleWallRuleTile = 6;
    }

    public override bool RuleMatch(int neighbor, TileBase tile) {
        switch (neighbor) {
            case Neighbor.This:    return tile == this || tile == caveRuleTile;
            case Neighbor.Nothing: return tile == null;
            case Neighbor.Any:     return tile != null;
            case Neighbor.TopRuleTile:        return IsUpGrassRuleTile( tile );
            case Neighbor.MiddleWallRuleTile: return IsMiddleWallRuleTile( tile );
        }
        return base.RuleMatch(neighbor, tile);
    }

    private bool IsUpGrassRuleTile( TileBase tile )
    {
        return topRuleTile == tile 
            || caveRuleTile == tile;
    }

    private bool IsMiddleWallRuleTile( TileBase tile )
    {
        return middleWallRuleTile == tile
            || caveRuleTile == tile;
    }
}