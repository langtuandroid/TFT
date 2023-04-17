using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TopRuleTile : RuleTile<TopRuleTile.Neighbor> {
    public TileBase caveRuleTile;
    public TileBase downWallRuleTile;
    public TileBase middleWallRuleTile;

    public class Neighbor : RuleTile.TilingRule.Neighbor {
        public const int Nothing = 3;
        public const int Any = 4;
        public const int DownWallRuleTile = 5;
        public const int MiddleWallRuleTile = 6;
    }

    public override bool RuleMatch(int neighbor, TileBase tile) {
        switch (neighbor) {
            case Neighbor.This:    return tile == this || tile == caveRuleTile;
            case Neighbor.Nothing: return tile == null;
            case Neighbor.Any:     return tile != null;
            case Neighbor.DownWallRuleTile:   return IsDownWallRuleTile( tile );
            case Neighbor.MiddleWallRuleTile: return IsMiddleWallRuleTile( tile );
        }
        return base.RuleMatch(neighbor, tile);
    }

    private bool IsMiddleWallRuleTile( TileBase tile )
    {
        return middleWallRuleTile == tile
            || caveRuleTile == tile;
    }

    private bool IsDownWallRuleTile( TileBase tile )
    {
        return downWallRuleTile == tile
            || caveRuleTile == tile;
    }
}