using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitStaticFunctions {
    public static float HexWidth { get; set; }
    public static float HexHeight { get; set; }
    public static Vector3 StartPos { get; set; }

    public static Vector3 CalcWorldPos(Vector2Int gridPos) {
        float offset = 0;
        if (gridPos.y % 2 != 0)
            offset = HexWidth / 2;

        float x = StartPos.x + gridPos.x * HexWidth + offset;
        float z = StartPos.z - gridPos.y * HexHeight * .75f;

        return new Vector3(x, 0, z);
    }

    public static Dictionary<Vector2Int, GameObject> Grid;
    public static Dictionary<UnitManager, Vector2Int> UnitPositions;

    public static Vector2Int GetGridPosFromWorldPos(GameObject valueVar) {
        foreach (Vector2Int keyVar in Grid.Keys) {
            if (Grid[keyVar] == valueVar) {
                return keyVar;
            }
        }
        return Vector2Int.zero;
    }

    public static UnitManager GetUnitFromGridPos(Vector2Int valueVar) {
        foreach (UnitManager keyVar in UnitPositions.Keys) {
            if (UnitPositions[keyVar] == valueVar) {
                return keyVar;
            }
        }
        return null;
    }
}