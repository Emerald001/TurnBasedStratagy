using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitComponents;

public abstract class UnitManager : MonoBehaviour
{
    [HideInInspector] public GameObject Unit;

    [HideInInspector] public TurnGameManager turnManager;
    
    [HideInInspector] public UnitDefineAccessableTiles defineAccessableTiles;
    [HideInInspector] public UnitDefineAttackableTiles defineAttackableTiles;
    [HideInInspector] public UnitPathfinding pathfinding;
    [HideInInspector] public HealthComponent health;

    [HideInInspector] public Vector2Int gridPos;
    [HideInInspector] public List<Vector2Int> currentPath = new List<Vector2Int>();
    [HideInInspector] public List<Vector2Int> AttackableTiles = new List<Vector2Int>();
    [HideInInspector] public List<Vector2Int> AccessableTiles = new List<Vector2Int>();
    [HideInInspector] public Dictionary<Vector2Int, Vector2Int> TileParents = new Dictionary<Vector2Int, Vector2Int>();
    [HideInInspector] public Dictionary<Vector2Int, GameObject> EnemyPositions = new Dictionary<Vector2Int, GameObject>();

    //UnitValues
    [HideInInspector] public int baseSpeedValue;
    [HideInInspector] public int baseInitiativeValue;
    [HideInInspector] public int baseDamageValue;

    [HideInInspector] public int speedValue;
    [HideInInspector] public int initiativeValue;
    [HideInInspector] public int damageValue;

    private UnitAction CurrentAction;
    private Queue<UnitAction> ActionQueue = new Queue<UnitAction>();
    
    public virtual void OnEnter() {
        Unit = this.gameObject;

        SetValues();

        pathfinding = new UnitPathfinding();

        defineAccessableTiles = new UnitDefineAccessableTiles();
        defineAccessableTiles.Owner = this;
        defineAttackableTiles = new UnitDefineAttackableTilesMelee();
        defineAttackableTiles.Owner = this;
        AccessableTiles = defineAccessableTiles.FindAccessableTiles(gridPos, speedValue, ref TileParents, turnManager.Tiles);
        AttackableTiles = defineAttackableTiles.FindAttackableTiles(turnManager.EnemyUnitsInPlay, EnemyPositions, turnManager.Tiles);
    }

    public virtual void SetValues() {
        speedValue = baseSpeedValue;
        initiativeValue = baseInitiativeValue;
        damageValue = baseDamageValue;

        //Values are defined here, to be made nice and open to call whenever
    }

    public virtual void OnUpdate() {
        CheckActionQueue();
    }

    private void CheckActionQueue() {
        if (CurrentAction == null && ActionQueue.Count > 0)
            CurrentAction = ActionQueue.Dequeue();
        else
            return;

        CurrentAction.OnUpdate();

        if (CurrentAction.IsDone) {
            if (ActionQueue.Count < 1) {
                CurrentAction = null;

                if (speedValue > 1) {
                    ResetTiles();
                    AccessableTiles = defineAccessableTiles.FindAccessableTiles(gridPos, speedValue, ref TileParents, turnManager.Tiles);
                    AttackableTiles = defineAttackableTiles.FindAttackableTiles(turnManager.EnemyUnitsInPlay, EnemyPositions, turnManager.Tiles);
                }
                return;
            }

            CurrentAction = ActionQueue.Dequeue();
        }
    }

    public virtual void OnExit() {
        defineAccessableTiles = null;
        defineAttackableTiles = null;
        pathfinding = null;

        ResetTiles();
    }

    public virtual void PickedTile(Vector2Int pickedTile) {
        if (AttackableTiles.Contains(pickedTile)) {
            if (gridPos == defineAttackableTiles.GetClosestTile(pickedTile, turnManager.blackBoard.HoverPoint, AccessableTiles)) {
                //ActionQueue.Enqueue(new UnitAttack(Unit, Enemy, currentDamageValue));
                ResetTiles();
            }
            else {
                currentPath = pathfinding.FindPathToTile(gridPos, defineAttackableTiles.GetClosestTile(pickedTile, turnManager.blackBoard.HoverPoint, AccessableTiles), TileParents);
                ActionQueue.Enqueue(new UnitMoveToTile(this));
                //ActionQueue.Enqueue(new UnitAttack(Unit, Enemy, currentDamageValue));
                ResetTiles();
            }
        }
        else if (AccessableTiles.Contains(pickedTile)) {
            currentPath = pathfinding.FindPathToTile(gridPos, pickedTile, TileParents);
            ActionQueue.Enqueue(new UnitMoveToTile(this));
            ResetTiles();
        }
    }

    public virtual void ResetTiles() {
        AccessableTiles.Clear();
        AttackableTiles.Clear();
        TileParents.Clear();
        currentPath.Clear();
    }
}