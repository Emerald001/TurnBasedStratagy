using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitComponents {
    public class UnitDefineAttackableTilesMelee : UnitDefineAttackableTiles {
        [HideInInspector] public UnitManager Owner;

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

        public override List<Vector2Int> FindAttackableTiles(List<Unit> AttackList, Dictionary<Vector2Int, GameObject> grid) {
            //clear previous attackable tiles!

            var attackableTiles = new List<Vector2Int>();

            for (int i = 0; i < Owner.AccessableTiles.Count; i++) {
                var currentPos = Owner.AccessableTiles[i];
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

                    foreach (Unit unit in AttackList) {
                        if (unit.gridPos == neighbour)
                            enemyInRange = true;
                    }

                    if (!grid[neighbour].CompareTag("WalkableTile") || Owner.AccessableTiles.Contains(neighbour) || !enemyInRange || attackableTiles.Contains(neighbour))
                        continue;

                    attackableTiles.Add(neighbour);
                }
            }

            return attackableTiles;
            
        }

        public override Vector2Int GetClosestTile(Vector2Int tile, Vector3 worldpoint, List<Vector2Int> accessableTiles) {
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

                if (!accessableTiles.Contains(neighbour))
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