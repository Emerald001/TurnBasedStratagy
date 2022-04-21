using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnvionment : MonoBehaviour
{
    //public Vector2Int GetClosestTile(Vector2Int tile, Vector3 worldpoint, List<Vector2Int> accessableTiles) {
    //    float smallestDistance = Mathf.Infinity;
    //    Vector2Int closestTile = Vector2Int.zero;

    //    var currentPos = tile;
    //    Vector2Int[] listToUse;

    //    if (currentPos.y % 2 != 0)
    //        listToUse = unevenNeighbours;
    //    else
    //        listToUse = evenNeighbours;

    //    for (int k = 0; k < 6; k++) {
    //        var neighbour = currentPos + listToUse[k];

    //        if (!accessableTiles.Contains(neighbour))
    //            continue;

    //        if (Vector3.Distance(UnitStaticFunctions.CalcWorldPos(neighbour), worldpoint) < smallestDistance) {
    //            smallestDistance = Vector3.Distance(UnitStaticFunctions.CalcWorldPos(neighbour), worldpoint);
    //            closestTile = neighbour;
    //        }
    //    }

    //    if (closestTile != Vector2Int.zero) {
    //        return closestTile;
    //    }
    //    return Vector2Int.zero;
    //}
}