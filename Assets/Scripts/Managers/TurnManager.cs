using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnitComponents;

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

    public Material WalkableTileColor;
    public Material AttackableTileColor;
    public Material ActiveUnitTileColor;

    [Header("References")]
    public Text InfoText;

    //shit it's given
    [HideInInspector] public List<UnitBase> PlayerUnitsToSpawn;
    [HideInInspector] public List<UnitBase> EnemiesToSpawn;

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
    [HideInInspector] public bool isDone; 

    void Start() {
        makeGrid = new MakeGrid(this, HexPrefab, ObstructedHexPrefab, gridWidth, gridHeight, gap, obstructedCellAmount);
        makeGrid.OnStart();
        UnitStaticFunctions.Grid = Tiles;

        //needs to be improved
        var camPos = transform.GetChild(0).transform;
        Camera.main.transform.SetPositionAndRotation(camPos.position, camPos.rotation);

        //spawn Player units and set them as child in an object
        var PlayerParent = new GameObject().transform;
        PlayerParent.name = "Player Units";
        PlayerParent.parent = this.transform;
        for (int i = 0; i < PlayerUnitsToSpawn.Count; i++) {
            SpawnUnits(UnitPrefab, PlayerUnitsToSpawn[i], PlayerUnitsInPlay, EnemyUnitsInPlay, true, PlayerParent, i, PlayerUnitsToSpawn.Count);
        }

        //spawn enemies and set them as child in an object
        var EnemyParent = new GameObject().transform;
        EnemyParent.name = "Enemy Units";
        EnemyParent.parent = this.transform;
        for (int i = 0; i < EnemiesToSpawn.Count; i++) {
            SpawnUnits(EnemyPrefab, EnemiesToSpawn[i], EnemyUnitsInPlay, PlayerUnitsInPlay, false, EnemyParent, i, EnemiesToSpawn.Count);
        }

        Tooltip.HideTooltip_Static();

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

    private void OnExit() {
        if (EnemyUnitsInPlay.Count < 1)
            InfoText.text = "Win!";
        else if(PlayerUnitsInPlay.Count < 1)
            InfoText.text = "Lose!";

        isDone = true;
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

        if (PlayerUnitsInPlay.Contains(unit)) {
            PlayerUnitsInPlay.Remove(unit);

            Debug.Log("Remove Player Unit from List, Player Units left: " + PlayerUnitsInPlay.Count);

            if (PlayerUnitsInPlay.Count < 1)
                OnExit();
        }
        else if (EnemyUnitsInPlay.Contains(unit)) {
            EnemyUnitsInPlay.Remove(unit);

            Debug.Log("Remove Enemy Unit from List, Enemy Units left: " + EnemyUnitsInPlay.Count);

            if (EnemyUnitsInPlay.Count < 1)
                OnExit();
        }
    }

    public void UnitWait() {
        if (CurrentUnit.values.initiativeValue < 1)
            return;

        var curUnit = CurrentUnit;
        NextUnit();
        UnitsWithTurnLeft.Add(curUnit);
        curUnit.values.initiativeValue *= -1;
        UpdateOrder();

        for (int i = 0; i < unitAttackOrder.Count; i++) {
            unitAttackOrder[i].gameObject.GetComponentInChildren<Text>().text = (i + 1).ToString();
        }
    }

    public void UnitEndTurn() {
        CurrentUnit.values.damageValue *= 2;
        NextUnit();
    }

    private void SpawnUnits(GameObject prefab, UnitBase values, List<UnitManager> listToAddTo, List<UnitManager> enemyList, bool spawnLeft, Transform unitParent, int index, int amount) {
        var gridPos = Vector2Int.zero;

        if (spawnLeft)
            gridPos = new Vector2Int(0, index + Mathf.RoundToInt((gridHeight / 2) - (amount / 2)));
        else
            gridPos = new Vector2Int(gridWidth - 1, index + Mathf.RoundToInt((gridHeight / 2) - (amount / 2)));

        var worldPos = Tiles[gridPos].transform.position;

        //create the Unit
        var Unit = GameObject.Instantiate(prefab, worldPos, Quaternion.identity);
        Unit.transform.parent = unitParent;
        Unit.AddComponent<HealthComponent>();

        var UnitScript = Unit.GetComponent<UnitManager>();

        //set Scripts
        if (values.isRanged)
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

        //Get values from Scriptable Object
        var unitValues = UnitScript.values;
        unitValues.baseDamageValue = values.baseDamageValue;
        unitValues.baseInitiativeValue = values.baseInitiativeValue;
        unitValues.baseSpeedValue = values.baseSpeedValue;
        unitValues.baseRangeValue = values.baseRangeValue;

        //run setvalues
        unitValues.SetValues();

        //give and set health
        var HealthScript = UnitScript.GetComponent<HealthComponent>();
        HealthScript.baseHealthValue = values.baseHealthValue;
        HealthScript.baseDefenceValue = values.baseDefenceValue;

        HealthScript.Defence = HealthScript.baseDefenceValue;
        HealthScript.Health = HealthScript.baseHealthValue;

        //add to lists for better accessability
        listToAddTo.Add(UnitScript);
        AllUnitsInPlay.Add(UnitScript);
        LivingUnitsInPlay.Add(UnitScript);
    }
}