using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTestManager : MonoBehaviour
{
    private MakeGrid makeGrid;
    private TurnManager turnM;

    public BattleSettings battleSettings;

    void Start() {
        turnM = GetComponent<TurnManager>();

        makeGrid = new(
            turnM, 
            battleSettings.HexPrefab, 
            battleSettings.ObstructedHexPrefab, 
            battleSettings.gridWidth, 
            battleSettings.gridHeight, 
            battleSettings.gap, 
            battleSettings.obstructedCellAmount
            );
    }
}