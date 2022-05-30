using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefineMulitpleTiles
{
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

    public List<Vector2Int> GetTiles(Vector2Int Point, Dictionary<Vector2Int, GameObject> grid) {
        var tiles = new List<Vector2Int>();

        Vector2Int[] listToUse;

        if (Point.y % 2 != 0)
            listToUse = unevenNeighbours;
        else
            listToUse = evenNeighbours;

        for (int i = 0; i < 6; i++) {
            var neighbour = Point + listToUse[i];

            if (!grid.ContainsKey(neighbour))
                continue;

            if (!grid[neighbour].CompareTag("WalkableTile"))
                continue;

            tiles.Add(neighbour);
        }

        return new List<Vector2Int>();
    }
}