using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitComponents;

public abstract class UnitManager : MonoBehaviour {
    [HideInInspector] public GameObject Unit;

    [HideInInspector] public TurnGameManager turnManager;
    [HideInInspector] public bool IsDone;

    [HideInInspector] public UnitDefineAccessableTiles defineAccessableTiles = new UnitDefineAccessableTiles();
    [HideInInspector] public UnitPathfinding pathfinding = new UnitPathfinding();
    [HideInInspector] public UnitValues values = new UnitValues();

    [HideInInspector] public UnitDefineAttackableTiles defineAttackableTiles;

    [HideInInspector] public Vector2Int gridPos;
    [HideInInspector] public List<UnitManager> EnemyList = new List<UnitManager>();
    [HideInInspector] public List<Vector2Int> CurrentPath = new List<Vector2Int>();
    [HideInInspector] public List<Vector2Int> AttackableTiles = new List<Vector2Int>();
    [HideInInspector] public List<Vector2Int> AccessableTiles = new List<Vector2Int>();
    [HideInInspector] public Dictionary<Vector2Int, Vector2Int> TileParents = new Dictionary<Vector2Int, Vector2Int>();
    [HideInInspector] public Dictionary<Vector2Int, GameObject> EnemyPositions = new Dictionary<Vector2Int, GameObject>();

    [HideInInspector] public List<UnitAbility> abilities = new List<UnitAbility>();

    private UnitAction CurrentAction;
    private Queue<UnitAction> ActionQueue = new Queue<UnitAction>();

    public virtual void OnEnter() {
        Unit = this.gameObject;

        values.SetValues();

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

        if (CurrentAction.IsDone) {
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
    }

    public virtual void OnExit() {
        values.initiativeValue = values.baseInitiativeValue;

        IsDone = false;

        ResetTiles();
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
        AccessableTiles = defineAccessableTiles.FindAccessableTiles(gridPos, values.speedValue, ref TileParents, turnManager.Tiles);
        AttackableTiles = defineAttackableTiles.FindAttackableTiles(gridPos, EnemyList, values.rangeValue, EnemyPositions, turnManager.Tiles);
    }

    public virtual void ResetTiles() {
        AccessableTiles.Clear();
        AttackableTiles.Clear();
        EnemyPositions.Clear();
        TileParents.Clear();
        CurrentPath.Clear();
    }

    public void AddEffect(UnitEffect effect) {
        values.Effects.Add(effect);
    }
}