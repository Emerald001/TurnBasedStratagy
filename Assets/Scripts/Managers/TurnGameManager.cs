using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnGameManager : MonoBehaviour
{
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

    public Material WalkableTileColor;
    public Material AttackableTileColor;

    public int unitAmount;
    public int enemyAmount;

    //Own vars
    [HideInInspector] public MakeGrid makeGrid;
    [HideInInspector] public BlackBoard blackBoard = new BlackBoard();
    [HideInInspector] public Dictionary<Vector2Int, GameObject> Tiles = new Dictionary<Vector2Int, GameObject>();

    [HideInInspector] public List<UnitManager> AllUnitsInPlay = new List<UnitManager>();
    [HideInInspector] public List<UnitManager> EnemyUnitsInPlay = new List<UnitManager>();
    [HideInInspector] public List<UnitManager> PlayerUnitsInPlay = new List<UnitManager>();
    [HideInInspector] public List<UnitManager> unitAttackOrder = new List<UnitManager>();
    [HideInInspector] public UnitManager CurrentUnit;

    void Start() {
        makeGrid = new MakeGrid(this, HexPrefab, ObstructedHexPrefab, gridWidth, gridHeight, gap, obstructedCellAmount);
        makeGrid.OnStart();
        UnitStaticFunctions.Grid = Tiles;

        //needs to be improved
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 10 + (gridHeight + gridWidth) / 10, -(10 + (gridHeight + gridWidth) / 10));

        SpawnPlayableUnits();

        UpdateOrder();

        NextUnit();
    }

    private void Update() {
        if (CurrentUnit != null) {
            CurrentUnit.OnUpdate();
            if (CurrentUnit.IsDone)
                NextUnit();
        }
    }

    private void SpawnPlayableUnits() {
        for (int i = 0; i < unitAmount; i++) {
            var gridPos = new Vector2Int(0, i + Mathf.RoundToInt((gridHeight / 2) - (unitAmount / 2)));
            var worldPos = Tiles[gridPos].transform.position;

            var Unit = GameObject.Instantiate(UnitPrefab, worldPos, Quaternion.identity);
            var UnitScript = Unit.GetComponent<UnitManager>();
            UnitScript.turnManager = this;
            UnitScript.gridPos = gridPos;

            UnitScript.baseDamageValue = Random.Range(1, 10);
            UnitScript.baseInitiativeValue = Random.Range(1, 10);
            UnitScript.baseSpeedValue = Random.Range(4, 7);

            UnitScript.SetValues();

            //var HealthScript = UnitScript.GetComponent<HealthComponent>();
            //HealthScript.baseHealthValue = Random.Range(10, 20);
            //HealthScript.baseDefenceValue = Random.Range(2, 7);

            //UnitScript.thisHealth = HealthScript;

            //HealthScript.DefenceValue = HealthScript.baseDefenceValue;
            //HealthScript.HealthValue = HealthScript.baseHealthValue;

            AllUnitsInPlay.Add(UnitScript);
            PlayerUnitsInPlay.Add(UnitScript);
        }
    }

    void NextUnit() {
        if (unitAttackOrder.Count != 0) {
            for (int i = 0; i < unitAttackOrder.Count; i++) {
                unitAttackOrder[i].gameObject.GetComponentInChildren<Text>().text = (i + 1).ToString();
            }

            if (CurrentUnit != null) {
                CurrentUnit.OnExit();
                CurrentUnit.gameObject.GetComponentInChildren<Text>().text = "-";
            }

            CurrentUnit = unitAttackOrder[0];
            unitAttackOrder.RemoveAt(0);
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
        unitAttackOrder = new List<UnitManager>(AllUnitsInPlay.OrderBy(x => x.initiativeValue).Reverse());

        for (int i = 0; i < unitAttackOrder.Count; i++) {
            unitAttackOrder[i].gameObject.GetComponentInChildren<Text>().text = (i + 1).ToString();
        }
    }
}