using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitComponents {
    public class UnitDefineAttackableTilesMelee : UnitDefineAttackableTiles {
        [HideInInspector]
        public Vector2Int[] evenNeighbours = {
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
            new Vector2Int(0, 1),
        };

        [HideInInspector]
        public Vector2Int[] unevenNeighbours = {
            new Vector2Int(0, -1),
            new Vector2Int(1, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(1, 1),
        };

        public override List<Vector2Int> FindAttackableTiles(UnitManager Owner, Vector2Int gridPos, List<UnitManager> AttackList, float range, Dictionary<Vector2Int, UnitManager> EnemyPositions, Dictionary<Vector2Int, GameObject> grid) {
            //clear previous attackable tiles!

            var attackableTiles = new List<Vector2Int>();
            var currentPos = gridPos;

            for (int i = 0; i < Owner.AccessableTiles.Count + 1; i++) {
                Vector2Int[] listToUse;

                if (currentPos.y % 2 != 0)
                    listToUse = unevenNeighbours;
                else
                    listToUse = evenNeighbours;

                for (int k = 0; k < 6; k++) {
                    var enemyInRange = false;
                    var neighbour = currentPos + listToUse[k];

                    if (!grid.ContainsKey(neighbour))
                        continue;

                    foreach (UnitManager unit in AttackList) {
                        if (unit.gridPos == neighbour) {
                            enemyInRange = true;
                            if(!EnemyPositions.ContainsKey(neighbour))
                                EnemyPositions.Add(neighbour, unit);
                        }
                    }

                    if (!grid[neighbour].CompareTag("WalkableTile") || Owner.AccessableTiles.Contains(neighbour) || !enemyInRange || attackableTiles.Contains(neighbour))
                        continue;

                    attackableTiles.Add(neighbour);
                }

                if(i !< Owner.AccessableTiles.Count)
                    currentPos = Owner.AccessableTiles[i];
            }

            return attackableTiles;
        }

        public override Vector2Int GetClosestTile(Vector2Int gridPos, Vector2Int tile, Vector3 worldpoint, List<Vector2Int> accessableTiles) {
            float smallestDistance = Mathf.Infinity;
            Vector2Int closestTile = Vector2Int.zero;

            var currentPos = tile;
            Vector2Int[] listToUse;

            if (currentPos.y % 2 != 0)
                listToUse = unevenNeighbours;
            else
                listToUse = evenNeighbours;

            for (int k = 0; k < 6; k++) {
                var neighbour = currentPos + listToUse[k];

                if (!accessableTiles.Contains(neighbour) && neighbour != gridPos)
                    continue;

                if (Vector3.Distance(UnitStaticFunctions.CalcWorldPos(neighbour), worldpoint) < smallestDistance) {
                    smallestDistance = Vector3.Distance(UnitStaticFunctions.CalcWorldPos(neighbour), worldpoint);
                    closestTile = neighbour;
                }
            }

            if (closestTile != Vector2Int.zero) {
                return closestTile;
            }
            return Vector2Int.zero;
        }
    }
}