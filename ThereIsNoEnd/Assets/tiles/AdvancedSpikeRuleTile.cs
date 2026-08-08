using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "2D/Tiles/AdvancedSpikes")]
public class AdvancedSpikeRuleTile : RuleTile<AdvancedSpikeRuleTile.Neighbor> {
    public bool customField;

    public class Neighbor : RuleTile.TilingRule.Neighbor {
        // unity base: public const int This = 1
        // unity base: public const int NotThis = 2
        public const int Wall = 3; //add wall tile so we can check against
        public const int Nothing = 4; //add nothing to check against
    }

    public override bool RuleMatch(int neighbor, TileBase tile) {
        switch (neighbor) {
            case Neighbor.Wall: return Check_Wall(tile);
            case Neighbor.Nothing: return Check_Nothing(tile);
        }
        return base.RuleMatch(neighbor, tile);
    }

    private bool Check_This(TileBase tile)
    {
        return tile == this;
    }

    private bool Check_Wall(TileBase tile)
    {
        return tile != this && tile != null;
    }

    private bool Check_Nothing(TileBase tile)
    {
        return tile == null;
    }
}