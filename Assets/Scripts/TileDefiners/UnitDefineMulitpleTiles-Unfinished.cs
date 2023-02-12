using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DefineMultipleTilesUnfinshed
{
    [HideInInspector]
    public static Vector2Int[] evenNeighbours = {
        new Vector2Int(-1, -1),
        new Vector2Int(-1, 1),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0),
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
    };

    [HideInInspector]
    public static Vector2Int[] unevenNeighbours = {
        new Vector2Int(0, -1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, 0),
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(1, 1),
    };

    public static List<Vector2Int> GetTiles(Vector2Int Point, int diamater, Dictionary<Vector2Int, GameObject> grid) {
        if (Point == Vector2Int.zero)
            return new List<Vector2Int>();

        var accessableList = new List<Vector2Int>();
        var openList = new List<Vector2Int>();
        var layerList = new List<Vector2Int>();
        var closedList = new List<Vector2Int>();

        if(grid.ContainsKey(Point))
            accessableList.Add(Point);

        openList.Add(Point);

        for (int i = 0; i < diamater; i++) {
            for (int j = 0; j < openList.Count; j++) {
                var currentPos = openList[j];
                Vector2Int[] listToUse;

                if (currentPos.y % 2 != 0)
                    listToUse = unevenNeighbours;
                else
                    listToUse = evenNeighbours;

                for (int k = 0; k < 6; k++) {
                    var neighbour = currentPos + listToUse[k];

                    if (!grid.ContainsKey(neighbour))
                        continue;

                    if (openList.Contains(neighbour) || closedList.Contains(neighbour) || layerList.Contains(neighbour))
                        continue;

                    if (!grid[neighbour].CompareTag("WalkableTile")){
                        layerList.Add(neighbour);
                        continue;
                    }

                    layerList.Add(neighbour);
                    accessableList.Add(neighbour);
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