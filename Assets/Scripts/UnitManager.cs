using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour {

    [Header("Grid Settings")]
    public GameObject HexPrefab;
    public GameObject ObstructedHexPrefab;

    public int gridWidth;
    public int gridHeight;
    public float gap;
    public int obstructedCellAmount;


    [Header("Units Settings")]
    public GameObject UnitPrefab;
    public GameObject EnemyPrefab;

    public Material ActiveUnitColor;
    public Material NormalUnitColor;
    public Material WalkableTileColor;
    public Material AttackableTileColor;

    public int unitAmount;
    public int enemyAmount;


    [Header("Own Vars")]
    public MakeGrid makeGrid;
    public BlackBoard blackBoard = new BlackBoard();
    public Dictionary<Vector2Int, GameObject> Tiles = new Dictionary<Vector2Int, GameObject>();

    public List<Unit> allUnits = new List<Unit>();
    public List<Unit> unitsInPlay = new List<Unit>();
    public List<Unit> enemiesInPlay = new List<Unit>();
    public Queue<Unit> unitAttackOrder = new Queue<Unit>();
    public Unit CurrentUnit;


    private void Start() {
        makeGrid = new MakeGrid(this, HexPrefab, ObstructedHexPrefab, gridWidth, gridHeight, gap, obstructedCellAmount);
        makeGrid.OnStart();

        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 10 + (gridHeight + gridWidth) / 10, -(10 + (gridHeight + gridWidth) / 10));

        SpawnUnits();
        SpawnEnemies();

        unitAttackOrder = new Queue<Unit>(allUnits.OrderBy(x => x.initiativeValue).Reverse());

        NextUnit();
    }

    // Update is called once per frame
    void Update() {
        if(CurrentUnit != null) {
            CurrentUnit.OnUpdate();
            if (Input.GetKeyDown(KeyCode.Space) || CurrentUnit.speedValue < 1)
                NextUnit();
        }
    }

    void SpawnUnits() {
        for (int i = 0; i < unitAmount; i++) {
            var gridPos = new Vector2Int(0, i + Mathf.RoundToInt((gridHeight / 2) - (unitAmount / 2)));
            var worldPos = Tiles[gridPos].transform.position;

            var tmp = GameObject.Instantiate(UnitPrefab, worldPos, Quaternion.identity).GetComponent<Unit>();
            tmp.Owner = this;
            tmp.gridPos = gridPos;
            tmp.baseInitiativeValue = Random.Range(1, 10);
            tmp.baseSpeedValue = Random.Range(2, 10);

            allUnits.Add(tmp);
            unitsInPlay.Add(tmp);
        }
    }

    void SpawnEnemies() {
        for (int i = 0; i < enemyAmount; i++) {
            var gridPos = new Vector2Int(gridWidth - 1, i + Mathf.RoundToInt((gridHeight / 2) - (unitAmount / 2)));
            var worldPos = Tiles[gridPos].transform.position;

            var tmp = GameObject.Instantiate(EnemyPrefab, worldPos, Quaternion.identity).GetComponent<Unit>();
            tmp.Owner = this;
            tmp.gridPos = gridPos;
            tmp.baseInitiativeValue = Random.Range(1, 10);
            tmp.baseSpeedValue = Random.Range(1, 7);

            allUnits.Add(tmp);
            enemiesInPlay.Add(tmp);
        }
    }

    void NextUnit() {
        if (unitAttackOrder.Count != 0) {
            if(CurrentUnit != null)
                CurrentUnit.OnExit();
            CurrentUnit = unitAttackOrder.Dequeue();
            CurrentUnit.OnEnter();
        }
        else {
            if (CurrentUnit != null)
                CurrentUnit.OnExit();
            CurrentUnit = null;
            NextTurn();
        }
    }

    void NextTurn() {
        UpdateOrder();
        NextUnit();
    }

    void UpdateOrder() {
        unitAttackOrder = new Queue<Unit>(allUnits.OrderBy(x => x.initiativeValue).Reverse());
    }

    public Vector2Int GetKeyFromValue(GameObject valueVar) {
        foreach (Vector2Int keyVar in Tiles.Keys) {
            if (Tiles[keyVar] == valueVar) {
                return keyVar;
            }
        }
        return Vector2Int.zero;
    }
}