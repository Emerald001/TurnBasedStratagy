using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitComponents { 
    public abstract class UnitDefineAttackableTilesUnfinished {

        public virtual List<Vector2Int> FindAttackableTiles(UnitManager Owner, Vector2Int gridPos, List<UnitManager> AttackList, float range, Dictionary<Vector2Int, UnitManager> EnemyPositions, Dictionary<Vector2Int, GameObject> grid) {
            //girdpos is unit's own Position
            //attacklist is a list of the enemy units
            //range is for the attackrange
            //enemypositions is to add the positions with enemies to, for attacking later on
            //grid is the entire grid, for checking neighbours and such

            return new List<Vector2Int>();
        }
        public virtual Vector2Int GetClosestTile(Vector2Int gridPos, Vector2Int tile, Vector3 worldpoint, List<Vector2Int> accessableTiles) {
            //gridpos is the unit's own positions
            //tile is the tile we want to get the closest pos of
            //worldpoint is the position of the mouse in realspace
            //accessable tiles is a list of tiles the unit could go

            return new Vector2Int();
        }
    }
}