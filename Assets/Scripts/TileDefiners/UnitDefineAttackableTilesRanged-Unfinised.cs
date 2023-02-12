using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitComponents {
    public class UnitDefineAttackableTilesRangedUnfinished : UnitDefineAttackableTiles {
        public override List<Vector2Int> FindAttackableTiles(UnitManager Owner, Vector2Int gridPos, List<UnitManager> AttackList, float range, Dictionary<Vector2Int, UnitManager> EnemyPositions, Dictionary<Vector2Int, GameObject> grid) {
            var attackableTiles = new List<Vector2Int>();

            for (int i = 0; i < AttackList.Count; i++) {
                if(Vector3.Distance(UnitStaticFunctions.CalcWorldPos(gridPos), UnitStaticFunctions.CalcWorldPos(AttackList[i].gridPos)) < range) {
                    attackableTiles.Add(AttackList[i].gridPos);
                    EnemyPositions.Add(AttackList[i].gridPos, AttackList[i]);
                }
            }

            return attackableTiles;
        }

        public override Vector2Int GetClosestTile(Vector2Int gridPos, Vector2Int tile, Vector3 worldpoint, List<Vector2Int> accessableTiles) {
            return gridPos;
        }
    }
}