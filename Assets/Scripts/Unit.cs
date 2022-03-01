using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public UnitManager Owner;
    public HealthComponent thisHealth;

    public bool IsWalking;
    public bool WillAttack;

    public int baseAttackValue;
    public int baseSpeedValue;
    public int baseInitiativeValue;

    public int attackValue;
    public int defenceValue;
    public int healthValue;
    public int speedValue;
    public int initiativeValue;    

    public List<Vector2Int> AccessableTiles = new List<Vector2Int>();
    public List<Vector2Int> AttackableTiles = new List<Vector2Int>();
    public Dictionary<Vector2Int, Vector2Int> TileParents = new Dictionary<Vector2Int, Vector2Int>();
    public List<Vector2Int> currentPath = new List<Vector2Int>();

    public Vector2Int gridPos;

    public Vector2Int[] evenNeighbours = {
        new Vector2Int(-1, -1),
        new Vector2Int(-1, 1),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0),
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
    };
    public Vector2Int[] unevenNeighbours = {
        new Vector2Int(0, -1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, 0),
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(1, 1),
    };

    public virtual void SetValues() {
        attackValue = baseAttackValue;
        speedValue = baseSpeedValue;
        initiativeValue = baseInitiativeValue;
    }

    public virtual void OnEnter() {
        SetValues();
    }

    public virtual void OnUpdate() {
        if(AccessableTiles.Count < 1 && speedValue > 0 && !IsWalking) {
            FindAccessableTiles();
        }
    }

    public abstract void OnExit();

    public virtual void FindAccessableTiles() {
        var openList = new List<Vector2Int>();
        var layerList = new List<Vector2Int>();
        var closedList = new List<Vector2Int>();

        openList.Add(gridPos);

        for (int i = 0; i < speedValue; i++) {
            for (int j = 0; j < openList.Count; j++) {
                var currentPos = openList[j];
                Vector2Int[] listToUse;

                if (currentPos.y % 2 != 0) 
                    listToUse = unevenNeighbours;
                else
                    listToUse = evenNeighbours;

                for (int k = 0; k < 6; k++) {
                    var skip = false;
                    var neighbour = currentPos + listToUse[k];

                    if(!Owner.Tiles.ContainsKey(neighbour))
                        continue;

                    foreach (Unit unit in Owner.allUnits) {
                        if(unit.gridPos == neighbour) 
                            skip = true;
                    }

                    if(!Owner.Tiles[neighbour].CompareTag("WalkableTile") || openList.Contains(neighbour) || closedList.Contains(neighbour) || layerList.Contains(neighbour) || skip)
                        continue;

                    layerList.Add(neighbour);
                    AccessableTiles.Add(neighbour);
                    TileParents.Add(neighbour, currentPos);
                }
                closedList.Add(openList[j]);
            }
            openList.Clear();
            for (int j = 0; j < layerList.Count; j++) {
                openList.Add(layerList[j]);
            }
            layerList.Clear();
        }
    }

    public virtual void FindMeleeAttackableTiles(List<Unit> AttackList) {
        AttackableTiles.Clear();

        for (int i = 0; i < AccessableTiles.Count; i++) {
            var currentPos = AccessableTiles[i];
            Vector2Int[] listToUse;

            if (currentPos.y % 2 != 0)
                listToUse = unevenNeighbours;
            else
                listToUse = evenNeighbours;

            for (int k = 0; k < 6; k++) {
                var enemyInRange = false;
                var neighbour = currentPos + listToUse[k];

                if (!Owner.Tiles.ContainsKey(neighbour))
                    continue;

                foreach (Unit unit in AttackList) {
                    if (unit.gridPos == neighbour)
                        enemyInRange = true;
                }

                if (!Owner.Tiles[neighbour].CompareTag("WalkableTile") || AccessableTiles.Contains(neighbour) || !enemyInRange || AttackableTiles.Contains(neighbour))
                    continue;

                AttackableTiles.Add(neighbour);
            }
        }
    }

    public virtual void ResetTiles() {
        AccessableTiles.Clear();
        TileParents.Clear();
    }

    public virtual void FindPathToTile(Vector2Int gridPos) {
        if (IsWalking)
            return;

        currentPath = null;
        var path = new List<Vector2Int>();
        var currentPos = gridPos;
        var breakoutCounter = speedValue * 2;

        while (currentPos != this.gridPos && breakoutCounter > 0) {
            path.Add(currentPos);
            currentPos = TileParents[currentPos];

            breakoutCounter++;
        }

        path.Add(currentPos);
        path.Reverse();

        currentPath = path;
    }

    public virtual void MoveToTile() {
        IsWalking = true;

        if (transform.position != Owner.makeGrid.CalcWorldPos(currentPath[0])) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Owner.makeGrid.CalcWorldPos(currentPath[0]) - transform.position), 360f * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, Owner.makeGrid.CalcWorldPos(currentPath[0]), 4 * Time.deltaTime);
        }
        else {
            gridPos = currentPath[0];
            speedValue--;
            currentPath.RemoveAt(0);

            if(currentPath.Count == 1) {
                AttackUnit();
            }
        }

        if(currentPath.Count < 1) {
            IsWalking = false;
        }
    }

    public virtual void AttackUnit() {
        bool enemyToAttack = false;
        Vector2Int enemyPos = Vector2Int.zero;

        foreach (Vector2Int pos in AttackableTiles) {
            if (currentPath[0] == pos) {
                enemyToAttack = true;
            }
        }

        if (!enemyToAttack)
            return;

        Unit enemy = null;

        foreach (Unit en in Owner.allUnits) {
            if (en.gridPos == currentPath[0])
                enemy = en;
        }

        enemy.GetComponent<HealthComponent>().TakeDamage(attackValue);
        currentPath.Clear();
        speedValue = 0;
        IsWalking = false;
    }
}