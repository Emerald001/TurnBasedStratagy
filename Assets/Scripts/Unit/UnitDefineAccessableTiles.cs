using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitComponents { 
    public class UnitDefineAccessableTiles {
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

        public virtual List<Vector2Int> FindAccessableTiles(UnitManager Owner, Vector2Int gridPos, int speedValue, ref Dictionary<Vector2Int, Vector2Int> parentDic, Dictionary<Vector2Int, GameObject> grid) {
            var accessableList = new List<Vector2Int>();
            var openList = new List<Vector2Int>();
            var layerList = new List<Vector2Int>();
            var closedList = new List<Vector2Int>();

            openList.Add(gridPos);

            for (int i = 0; i < speedValue; i++) {
                for (int j = 0; j < openList.Count; j++) {
                    var currentPos = openList[j];
                    Vector2Int[] listToUse;

                    if (currentPos.y % 2 != 0)
                        listToUse = unevenNeighbours;
                    else
                        listToUse = evenNeighbours;

                    for (int k = 0; k < 6; k++) {
                        var skip = false;
                        var neighbour = currentPos + listToUse[k];

                        if (!grid.ContainsKey(neighbour))
                            continue;

                        foreach (UnitManager unit in Owner.turnManager.AllUnitsInPlay) {
                            if (unit.gridPos == neighbour)
                                skip = true;
                        }

                        if (!grid[neighbour].CompareTag("WalkableTile") || openList.Contains(neighbour) || closedList.Contains(neighbour) || layerList.Contains(neighbour) || skip)
                            continue;

                        layerList.Add(neighbour);
                        accessableList.Add(neighbour);
                        parentDic.Add(neighbour, currentPos);
                    }
                    closedList.Add(openList[j]);
                }
                openList.Clear();
                for (int j = 0; j < layerList.Count; j++) {
                    openList.Add(layerList[j]);
                }
                layerList.Clear();
            }
            return accessableList;
        }
    }
}