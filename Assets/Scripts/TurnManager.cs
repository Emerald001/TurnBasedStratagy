using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour {

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


    //Own vars
    [HideInInspector] public MakeGrid makeGrid;
    [HideInInspector] public BlackBoard blackBoard = new BlackBoard();
    [HideInInspector] public Dictionary<Vector2Int, GameObject> Tiles = new Dictionary<Vector2Int, GameObject>();

    [HideInInspector] public List<Unit> AllUnitsInPlay = new List<Unit>();
    [HideInInspector] public List<Unit> EnemyUnitsInPlay = new List<Unit>();
    [HideInInspector] public List<Unit> PlayerUnitsInPlay = new List<Unit>();
    [HideInInspector] public Queue<Unit> unitAttackOrder = new Queue<Unit>();
    [HideInInspector] public Unit CurrentUnit;
    
    private void Start() {
        makeGrid = new MakeGrid(this, HexPrefab, ObstructedHexPrefab, gridWidth, gridHeight, gap, obstructedCellAmount);
        makeGrid.OnStart();

        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 10 + (gridHeight + gridWidth) / 10, -(10 + (gridHeight + gridWidth) / 10));

        SpawnUnits();
        SpawnEnemies();

        UpdateOrder();

        NextUnit();
    }

    private void Update() {
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

            var UnitScript = GameObject.Instantiate(UnitPrefab, worldPos, Quaternion.identity).GetComponent<Unit>();
            UnitScript.Owner = this;
            UnitScript.gridPos = gridPos;
            UnitScript.baseAttackValue = Random.Range(1, 10);
            UnitScript.baseInitiativeValue = Random.Range(1, 10);
            UnitScript.baseSpeedValue = Random.Range(4, 7);

            var HealthScript = UnitScript.GetComponent<HealthComponent>();
            HealthScript.baseHealthValue = Random.Range(10, 20);
            HealthScript.baseDefenceValue = Random.Range(2, 7);

            UnitScript.thisHealth = HealthScript;

            HealthScript.DefenceValue = HealthScript.baseDefenceValue;
            HealthScript.HealthValue = HealthScript.baseHealthValue;

            UnitScript.SetValues();

            AllUnitsInPlay.Add(UnitScript);
            PlayerUnitsInPlay.Add(UnitScript);
        }
    }

    void SpawnEnemies() {
        for (int i = 0; i < enemyAmount; i++) {
            var gridPos = new Vector2Int(gridWidth - 1, i + Mathf.RoundToInt((gridHeight / 2) - (unitAmount / 2)));
            var worldPos = Tiles[gridPos].transform.position;

            var UnitScript = GameObject.Instantiate(EnemyPrefab, worldPos, Quaternion.identity).GetComponent<Unit>();
            UnitScript.Owner = this;
            UnitScript.gridPos = gridPos;
            UnitScript.baseAttackValue = Random.Range(1, 10);
            UnitScript.baseInitiativeValue = Random.Range(1, 10);
            UnitScript.baseSpeedValue = Random.Range(1, 7);

            var HealthScript = UnitScript.GetComponent<HealthComponent>();
            HealthScript.baseHealthValue = Random.Range(10, 20);
            HealthScript.baseDefenceValue = Random.Range(2, 7);

            UnitScript.thisHealth = HealthScript;

            HealthScript.DefenceValue = HealthScript.baseDefenceValue;
            HealthScript.HealthValue = HealthScript.baseHealthValue;

            UnitScript.SetValues();

            AllUnitsInPlay.Add(UnitScript);
            EnemyUnitsInPlay.Add(UnitScript);
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
        unitAttackOrder = new Queue<Unit>(AllUnitsInPlay.OrderBy(x => x.initiativeValue).Reverse());
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