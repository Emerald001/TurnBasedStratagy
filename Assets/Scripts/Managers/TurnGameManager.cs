using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnitComponents;

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

    [Header("References")]
    public Text InfoText;

    //Own vars
    [HideInInspector] public MakeGrid makeGrid;
    [HideInInspector] public Dictionary<Vector2Int, GameObject> Tiles = new Dictionary<Vector2Int, GameObject>();

    [HideInInspector] public List<UnitManager> AllUnitsInPlay = new List<UnitManager>();
    [HideInInspector] public List<UnitManager> UnitsWithTurnLeft = new List<UnitManager>();
    [HideInInspector] public List<UnitManager> LivingUnitsInPlay = new List<UnitManager>();
    [HideInInspector] public List<UnitManager> DeadUnitsInPlay = new List<UnitManager>();
    [HideInInspector] public List<UnitManager> EnemyUnitsInPlay = new List<UnitManager>();
    [HideInInspector] public List<UnitManager> PlayerUnitsInPlay = new List<UnitManager>();

    [HideInInspector] public List<UnitManager> unitAttackOrder = new List<UnitManager>();
    [HideInInspector] public UnitManager CurrentUnit;

    private int CurrentTurn;

    void Start() {
        makeGrid = new MakeGrid(this, HexPrefab, ObstructedHexPrefab, gridWidth, gridHeight, gap, obstructedCellAmount);
        makeGrid.OnStart();
        UnitStaticFunctions.Grid = Tiles;

        //needs to be improved
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 10 + (gridHeight + gridWidth) / 10, -(10 + (gridHeight + gridWidth) / 10));

        SpawnUnits(UnitPrefab, PlayerUnitsInPlay, EnemyUnitsInPlay, true);
        SpawnUnits(EnemyPrefab, EnemyUnitsInPlay, PlayerUnitsInPlay, false);

        NextTurn();
    }

    private void Update() {
        if (CurrentUnit != null) {
            CurrentUnit.OnUpdate();

            if (CurrentUnit.IsDone)
                NextUnit();
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            UnitWait();
        }
    }

    private void SpawnUnits(GameObject prefab, List<UnitManager> listToAddTo, List<UnitManager> enemyList, bool spawnLeft) {
        for (int i = 0; i < unitAmount; i++) {
            var gridPos = Vector2Int.zero;
            bool isRanged = Random.Range(0, 2) != 0;

            if(spawnLeft)
                gridPos = new Vector2Int(0, i + Mathf.RoundToInt((gridHeight / 2) - (unitAmount / 2)));
            else
                gridPos = new Vector2Int(gridWidth - 1, i + Mathf.RoundToInt((gridHeight / 2) - (unitAmount / 2)));

            var worldPos = Tiles[gridPos].transform.position;

            //create the Unit
            var Unit = GameObject.Instantiate(prefab, worldPos, Quaternion.identity);
            Unit.AddComponent<HealthComponent>();

            var UnitScript = Unit.GetComponent<UnitManager>();

            //set Scripts
            if (isRanged)
                UnitScript.defineAttackableTiles = new UnitDefineAttackableTilesRanged();
            else
                UnitScript.defineAttackableTiles = new UnitDefineAttackableTilesMelee();

            UnitScript.pathfinding = new UnitPathfinding();
            UnitScript.defineAccessableTiles = new UnitDefineAccessableTiles();
            UnitScript.defineAccessableTiles.Owner = UnitScript;
            UnitScript.defineAttackableTiles.Owner = UnitScript;

            //Give Values
            UnitScript.turnManager = this;
            UnitScript.gridPos = gridPos;
            UnitScript.EnemyList = enemyList;

            //Random values -- Will change in replacement with scriptable Objects
            var values = UnitScript.values;
            values.baseDamageValue = Random.Range(5, 10);
            values.baseInitiativeValue = Random.Range(1, 10);
            values.baseSpeedValue = Random.Range(4, 7);
            values.baseRangeValue = 100;

            //run setvalues
            values.SetValues();

            //give health
            var HealthScript = UnitScript.GetComponent<HealthComponent>();
            HealthScript.baseHealthValue = Random.Range(70, 100);
            HealthScript.baseDefenceValue = Random.Range(2, 7);

            HealthScript.Defence = HealthScript.baseDefenceValue;
            HealthScript.Health = HealthScript.baseHealthValue;

            listToAddTo.Add(UnitScript);
            AllUnitsInPlay.Add(UnitScript);
            LivingUnitsInPlay.Add(UnitScript);
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
            UnitsWithTurnLeft.Remove(CurrentUnit);
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
        foreach (var unit in LivingUnitsInPlay) {
            UnitsWithTurnLeft.Add(unit);
        }
        
        CurrentTurn++;
        InfoText.text = "Turn " + CurrentTurn.ToString();

        UpdateOrder();
        NextUnit();
    }

    void UpdateOrder() {
        unitAttackOrder = new List<UnitManager>(UnitsWithTurnLeft.OrderBy(x => x.values.initiativeValue).Reverse());
    }

    public void UnitDeath(UnitManager unit) {
        LivingUnitsInPlay.Remove(unit);
        DeadUnitsInPlay.Add(unit);

        if (unitAttackOrder.Contains(unit)) {
            UnitsWithTurnLeft.Remove(unit);
            unitAttackOrder.Remove(unit);
        }

        UpdateOrder();

        if (PlayerUnitsInPlay.Contains(unit))
            PlayerUnitsInPlay.Remove(unit);
        else if (EnemyUnitsInPlay.Contains(unit))
            EnemyUnitsInPlay.Remove(unit);
    }

    public void UnitWait() {
        if (CurrentUnit.values.initiativeValue < 1)
            return;

        var curUnit = CurrentUnit;
        NextUnit();
        UnitsWithTurnLeft.Add(curUnit);
        curUnit.values.initiativeValue *= -1;
        UpdateOrder();
    }
}