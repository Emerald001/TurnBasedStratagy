using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BattleSettings
{
    [Header("Grid Settings")]
    public static GameObject HexPrefab;
    public static GameObject ObstructedHexPrefab;

    public static int gridWidth;
    public static int gridHeight;
    public static float gap;
    public static int obstructedCellAmount;

    [Header("Units Settings")]
    public static GameObject UnitPrefab;
    public static GameObject EnemyPrefab;

    public static Material WalkableTileColor;
    public static Material AttackableTileColor;
    public static Material ActiveUnitTileColor;

    [Header("References")]
    public static GameObject UI;
}