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
    public GameObject InfoText;
    public List<GameObject> Buttons;

    //shit it's given
    [HideInInspector] public List<UnitBase> PlayerUnitsToSpawn;
    [HideInInspector] public List<UnitBase> EnemiesToSpawn;

    //Own vars
    [HideInInspector] public MakeGrid makeGrid;
    [HideInInspector] public TurnUIManager UIManager;
    [HideInInspector] public UnitSpawn SpawnUnits = new UnitSpawn();
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

    private void Start() {
        makeGrid = new MakeGrid(this, HexPrefab, ObstructedHexPrefab, gridWidth, gridHeight, gap, obstructedCellAmount);
        makeGrid.OnStart();
        UnitStaticFunctions.Grid = Tiles;

        UIManager = new TurnUIManager(Buttons, InfoText);

        //needs to be improved
        var camPos = transform.GetChild(0).transform;
        Camera.main.transform.SetPositionAndRotation(camPos.position, camPos.rotation);

        //spawn Player units and set them as child in an object
        var PlayerParent = new GameObject().transform;
        PlayerParent.name = "Player Units";
        PlayerParent.parent = this.transform;
        for (int i = 0; i < PlayerUnitsToSpawn.Count; i++) {
            var gridPos = new Vector2Int(0, i + Mathf.RoundToInt((gridHeight / 2) - (PlayerUnitsToSpawn.Count / 2)));
            SpawnUnits.SpawnUnits(this, UnitPrefab, PlayerUnitsToSpawn[i], gridPos, PlayerUnitsInPlay, EnemyUnitsInPlay, PlayerParent);
        }

        //spawn enemies and set them as child in an object
        var EnemyParent = new GameObject().transform;
        EnemyParent.name = "Enemy Units";
        EnemyParent.parent = this.transform;
        for (int i = 0; i < EnemiesToSpawn.Count; i++) {
            var gridPos = new Vector2Int(gridWidth - 1, i + Mathf.RoundToInt((gridHeight / 2) - (EnemiesToSpawn.Count / 2)));
            SpawnUnits.SpawnUnits(this, EnemyPrefab, EnemiesToSpawn[i], gridPos, EnemyUnitsInPlay, PlayerUnitsInPlay, EnemyParent);
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
            UIManager.SetInfoText("Win!");
        else if(PlayerUnitsInPlay.Count < 1)
            UIManager.SetInfoText("Lose!");

        isDone = true;
    }

    private void NextUnit() {
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

    private void NextTurn() {
        foreach (var unit in LivingUnitsInPlay) {
            UnitsWithTurnLeft.Add(unit);
        }

        CurrentTurn++;

        UIManager.SetInfoText("Turn " + CurrentTurn.ToString());

        UpdateOrder();
        NextUnit();
    }

    private void UpdateOrder() {
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

            if (PlayerUnitsInPlay.Count < 1)
                OnExit();
        }
        else if (EnemyUnitsInPlay.Contains(unit)) {
            EnemyUnitsInPlay.Remove(unit);

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
            unitAttackOrder[i].gameObject.GetComponentInChildren<Text>().text = (i + 2).ToString();
        }
    }

    public void UnitEndTurn() {
        CurrentUnit.values.damageValue *= 2;
        NextUnit();
    }
}