using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeGrid {
    private TurnManager Owner;
    private GameObject Parent;

    private GameObject HexPrefab;
    private GameObject ObstructedHexPrefab;

    private int gridWidth;
    private int gridHeight;
    private float gap;
    private int emptyCellAmount;

    private float hexWidth = 1.732f;
    private float hexHeight = 2f;

    public MakeGrid(TurnManager Owner, GameObject HexPrefab, GameObject ObstructedHexPrefab, int gridWidth, int gridHeight, float gap, int obsticalAmount) {
        this.Owner = Owner;

        this.HexPrefab = HexPrefab;
        this.ObstructedHexPrefab = ObstructedHexPrefab;

        this.gap = gap;
        AddGap();

        this.gridWidth = gridWidth;
        this.gridHeight = gridHeight;
        CalcStartPos();
        
        this.emptyCellAmount = obsticalAmount;

        UnitStaticFunctions.HexHeight = hexHeight;
        UnitStaticFunctions.HexWidth = hexWidth;
        UnitStaticFunctions.StartPos = startpos;
    }

    private List<Vector2Int> emptyCells = new List<Vector2Int>();
    private List<Vector2Int> obstructedCells = new List<Vector2Int>();
    private List<Vector2Int> treasureCells = new List<Vector2Int>();

    private Vector3 startpos;

    public void OnStart() {
        Parent = new GameObject();
        Parent.name = "Grid";

        TrimCorners();
        DefineObstacle();
        CreateGrid();
    }

    void AddGap() {
        hexWidth += hexWidth * gap;
        hexHeight += hexHeight * gap;
    }

    void CalcStartPos() {
        float offset = 0;
        if (gridHeight / 2 % 2 != 0)
            offset = hexWidth / 2;

        float x = -hexWidth * (gridWidth / 2) - offset;
        float z = hexHeight * .75f * (gridHeight / 2);

        startpos = new Vector3(x, 0, z);
    }

    void TrimCorners() {
        if (gridHeight / 2 % 2 != 0) {
            emptyCells.Add(new Vector2Int(0, 0));
            emptyCells.Add(new Vector2Int(0, gridHeight - 1));
        }
        else {
            emptyCells.Add(new Vector2Int(0, 0));
            emptyCells.Add(new Vector2Int(gridWidth - 1, gridHeight - 1));
        }
    }

    void CreateGrid() {
        for (int Y = 0; Y < gridHeight; Y++) {
            for (int X = 0; X < gridWidth; X++) {
                Vector2Int gridPos = new Vector2Int(X, Y);
                if (emptyCells.Contains(gridPos) || obstructedCells.Contains(gridPos)) {
                    continue;
                }
                GameObject hex = GameObject.Instantiate(HexPrefab);
                hex.transform.position = UnitStaticFunctions.CalcWorldPos(gridPos);
                hex.transform.parent = Parent.transform;
                hex.name = "Hexagon " + X + "|" + Y;
                Owner.Tiles.Add(gridPos, hex);
            }
        }

        for (int i = 0; i < obstructedCells.Count; i++) {
            GameObject hex = GameObject.Instantiate(ObstructedHexPrefab);
            hex.transform.position = UnitStaticFunctions.CalcWorldPos(obstructedCells[i]);
            hex.transform.parent = Parent.transform;
            hex.name = "Obstructed Hexagon " + obstructedCells[i].x + "|" + obstructedCells[i].y;
            Owner.Tiles.Add(obstructedCells[i], hex);
        }
    }
    
    void DefineObstacle() {
        var counter = 0;

        while (counter < emptyCellAmount) {
            var tmp = new Vector2Int(Random.Range(2, gridWidth - 3), Random.Range(0, gridHeight));
            if (obstructedCells.Contains(tmp))
                continue;
            if (tmp.y < 1 || tmp.y > gridHeight - 2) {
                emptyCells.Add(tmp);
                counter++;
                continue;
            }
            obstructedCells.Add(tmp);
            counter++;
        }
    }
}