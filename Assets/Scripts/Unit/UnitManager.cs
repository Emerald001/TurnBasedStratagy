using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitComponents;

public abstract class UnitManager : MonoBehaviour {
    [HideInInspector] public GameObject Unit;

    [HideInInspector] public TurnManager turnManager;
    [HideInInspector] public HealthComponent HealthComponent;
    [HideInInspector] public bool IsDone;

    [HideInInspector] public UnitDefineAccessableTiles defineAccessableTiles = new UnitDefineAccessableTiles();
    [HideInInspector] public UnitPathfinding pathfinding = new UnitPathfinding();
    [HideInInspector] public UnitValues values;

    [HideInInspector] public UnitDefineAttackableTiles defineAttackableTiles;

    [HideInInspector] public Vector2Int gridPos;
    [HideInInspector] public List<UnitManager> EnemyList = new List<UnitManager>();
    [HideInInspector] public List<UnitManager> OwnList = new List<UnitManager>();
    [HideInInspector] public List<Vector2Int> CurrentPath = new List<Vector2Int>();
    [HideInInspector] public List<Vector2Int> AttackableTiles = new List<Vector2Int>();
    [HideInInspector] public List<Vector2Int> AccessableTiles = new List<Vector2Int>();
    [HideInInspector] public Dictionary<Vector2Int, Vector2Int> TileParents = new Dictionary<Vector2Int, Vector2Int>();
    [HideInInspector] public Dictionary<Vector2Int, UnitManager> EnemyPositions = new Dictionary<Vector2Int, UnitManager>();

    [HideInInspector] public List<AbilityBase> abilities = new List<AbilityBase>();
    [HideInInspector] public AbilityBase pickedAbility;

    [HideInInspector] public Queue<UnitAction> ActionQueue = new Queue<UnitAction>();
    private UnitAction CurrentAction;

    public virtual void OnEnter() {
        Unit = this.gameObject;

        FindTiles();
    }

    public virtual void OnUpdate() {
        CheckActionQueue();
    }

    private void CheckActionQueue() {
        if (CurrentAction == null && ActionQueue.Count > 0)
            CurrentAction = ActionQueue.Dequeue();
        else if (CurrentAction == null)
            return;

        CurrentAction.OnUpdate();

        if (!CurrentAction.IsDone)
            return;
        
        if (ActionQueue.Count < 1) {
            if (CurrentAction.EndsTurn) {
                CurrentAction = null;
                IsDone = true;

                return;
            }
            else {
                CurrentAction = null;

                FindTiles();

                return;
            }
        }

        CurrentAction = ActionQueue.Dequeue();
    }

    public virtual void OnExit() {
        ResetTiles();
        
        if (pickedAbility != null)
            pickedAbility.OnExit();
        
        values.initiativeValue = values.baseInitiativeValue;
        pickedAbility = null;
        IsDone = false;
    }

    public virtual void PickedTile(Vector2Int pickedTile, Vector2Int standingPos_optional) {
        if (AttackableTiles.Contains(pickedTile)) {
            if (gridPos == standingPos_optional) {
                ActionQueue.Enqueue(new UnitAttack(Unit, EnemyPositions[pickedTile], values.damageValue));
                ResetTiles();
            }
            else {
                ActionQueue.Enqueue(new UnitMoveToTile(this, pathfinding.FindPathToTile(gridPos, standingPos_optional, TileParents)));
                ActionQueue.Enqueue(new UnitAttack(Unit, EnemyPositions[pickedTile], values.damageValue));
                ResetTiles();
            }
        }
        else if (AccessableTiles.Contains(pickedTile)) {
            ActionQueue.Enqueue(new UnitMoveToTile(this, pathfinding.FindPathToTile(gridPos, pickedTile, TileParents)));
            ResetTiles();
        }
    }

    public virtual void FindTiles() {
        AccessableTiles = defineAccessableTiles.FindAccessableTiles(this, gridPos, values.speedValue, ref TileParents, turnManager.Tiles);
        AttackableTiles = defineAttackableTiles.FindAttackableTiles(this, gridPos, EnemyList, values.rangeValue, EnemyPositions, turnManager.Tiles);
    }

    public virtual void ResetTiles() {
        AccessableTiles.Clear();
        AttackableTiles.Clear();
        EnemyPositions.Clear();
        TileParents.Clear();
        CurrentPath.Clear();
        pickedAbility = null;
    }

    public void AddEffect(UnitEffect effect, int valueChanged, int duration) {
        values.ApplyEffect(effect, valueChanged, duration);
    }

    public void LowerAbilityCooldowns() {
        foreach (var ability in abilities) {
            if (ability.currentCooldown > 0)
                ability.currentCooldown--;
        }
    }

    public virtual void SelectAbility(int index) {
        if (pickedAbility == abilities[index]) {
            pickedAbility.OnExit();
            pickedAbility = null;
        }
        else {
            if(pickedAbility != null) {
                pickedAbility.OnExit();
            }
            pickedAbility = abilities[index];

            if (pickedAbility.targetAnything)
                pickedAbility.OnEnter(this, turnManager.AllUnitsInPlay);
            else if (pickedAbility.targetEnemy)
                pickedAbility.OnEnter(this, EnemyList);
            else
                pickedAbility.OnEnter(this, OwnList);
        }
    }
}