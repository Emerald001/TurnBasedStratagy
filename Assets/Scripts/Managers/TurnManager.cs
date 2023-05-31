using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnitComponents;

public class TurnManager : MonoBehaviour {
    [Header("BattleSettingsScriptableObject")]
    [SerializeField] private BattleSettings battleSettings;

    [Header("References")]
    [SerializeField] private List<GameObject> Buttons;
    [SerializeField] private GameObject InfoText;
    [SerializeField] private GameObject EndScreen;
    [SerializeField] private GameObject Text;

    // Private vars.
    private readonly List<UnitManager> UnitsWithTurnLeft = new();
    private readonly List<UnitManager> DeadUnitsInPlay = new();
    private readonly List<UnitManager> EnemyUnitsInPlay = new();
    private readonly List<UnitManager> PlayerUnitsInPlay = new();

    private List<UnitManager> livingUnitsInPlay = new();
    private List<UnitManager> unitAttackOrder = new();

    private UnitManager CurrentUnit;
    
    private int CurrentTurn;

    // Properties
    public Dictionary<UnitManager, Vector2Int> UnitPositions { get; private set; } = new();
    public Dictionary<Vector2Int, GameObject> Tiles { get; private set; } = new();
    public List<UnitBase> PlayerUnitsToSpawn { get; set; }
    public List<UnitBase> EnemiesToSpawn { get; set; }
    public List<UnitManager> LivingUnitsInPlay { get => livingUnitsInPlay; set => livingUnitsInPlay = value; }
    public List<UnitManager> AllUnitsInPlay { get; private set; } = new();
    public BattleSettings BattleSettings { get { return battleSettings; } private set { } }
    public TurnUIManager UIManager { get; private set; }
    public bool IsDone { get; set; }

    private void Start() {
        new MakeGrid(
            this,
            BattleSettings.HexPrefab,
            BattleSettings.ObstructedHexPrefab,
            BattleSettings.gridWidth,
            BattleSettings.gridHeight,
            BattleSettings.gap,
            BattleSettings.obstructedCellAmount
            );

        UnitStaticFunctions.Grid = Tiles;
        UnitStaticFunctions.UnitPositions = UnitPositions;

        UIManager = new TurnUIManager(this, Buttons, InfoText, EndScreen);
        UIManager.DeactivateButtons();

        //needs to be improved
        var camPos = transform.GetChild(0).transform;
        Camera.main.transform.SetPositionAndRotation(camPos.position, camPos.rotation);

        //spawn Player units and set them as child on an object
        var PlayerParent = new GameObject().transform;
        PlayerParent.name = "Player Units";
        PlayerParent.parent = transform;
        for (int i = 0; i < PlayerUnitsToSpawn.Count; i++) {
            var gridPos = new Vector2Int(0, 
                i + Mathf.RoundToInt((BattleSettings.gridHeight / 2) - (PlayerUnitsToSpawn.Count / 2)));
            
            UnitSpawn.SpawnUnits(this, 
                BattleSettings.UnitPrefab, 
                PlayerUnitsToSpawn[i], 
                gridPos, 
                PlayerUnitsInPlay, 
                EnemyUnitsInPlay, 
                PlayerParent
                );
        }

        //spawn enemies and set them as child in an object
        var EnemyParent = new GameObject().transform;
        EnemyParent.name = "Enemy Units";
        EnemyParent.parent = transform;
        for (int i = 0; i < EnemiesToSpawn.Count; i++) {
            var gridPos = new Vector2Int(BattleSettings.gridWidth - 1, 
                i + Mathf.RoundToInt((BattleSettings.gridHeight / 2) - (EnemiesToSpawn.Count / 2)));
            
            UnitSpawn.SpawnUnits(this, 
                BattleSettings.EnemyPrefab, 
                EnemiesToSpawn[i], 
                gridPos, 
                EnemyUnitsInPlay, 
                PlayerUnitsInPlay, 
                EnemyParent
                );
        }

        Tooltip.HideTooltip_Static();
        NextTurn();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Mouse0))
            Text.SetActive(false);

        if (CurrentUnit == null)
            return;

        if (CurrentUnit.IsDone) {
            NextUnit();
            return;
        }
            
        CurrentUnit.OnUpdate();
    }

    private void OnExit() {
        if (EnemyUnitsInPlay.Count < 1) {
            var header = "Win";
            var body = "You won in " + CurrentTurn + " turns.";

            UIManager.ShowEndScreen(header, body);
        }
        else if (PlayerUnitsInPlay.Count < 1) {
            var header = "Lose";
            var body = "You lost in " + CurrentTurn + " turns.";

            UIManager.ShowEndScreen(header, body);
        }
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
        for (int i = LivingUnitsInPlay.Count - 1; i >= 0; i--) {
            LivingUnitsInPlay[i].values.SetValues();
        }
        
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
                StartCoroutine(ExitDelay(2));
        }
        else if (EnemyUnitsInPlay.Contains(unit)) {
            EnemyUnitsInPlay.Remove(unit);

            if (EnemyUnitsInPlay.Count < 1)
                StartCoroutine(ExitDelay(2));
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
        CurrentUnit.values.defenceValue *= 2;
        NextUnit();
    }

    public IEnumerator ExitDelay(float time) {
        CurrentUnit = null;

        yield return new WaitForSeconds(time);
        OnExit();
    }
}