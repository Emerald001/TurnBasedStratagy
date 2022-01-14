using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public UnitManager Owner;

    public bool IsWalking;

    public int baseAttackValue;
    public int baseDefenceValue;
    public int baseHealthValue;
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

    public virtual void OnEnter() {
        attackValue = baseAttackValue;
        defenceValue = baseDefenceValue;
        healthValue = baseHealthValue;
        speedValue = baseSpeedValue;
        initiativeValue = baseInitiativeValue;
    }

    public abstract void OnUpdate();

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

            for (int j = 0; j < layerList.Count; j++) {
                openList.Add(layerList[j]);
            }
            layerList.Clear();
        }
    }

    public virtual void FindAttackableTiles(List<Unit> list) {
        for (int i = 0; i < AccessableTiles.Count; i++) {
            var currentPos = AccessableTiles[i];
            Vector2Int[] listToUse;

            if (currentPos.y % 2 != 0)
                listToUse = unevenNeighbours;
            else
                listToUse = evenNeighbours;

            for (int k = 0; k < 6; k++) {
                var skip = false;
                var neighbour = currentPos + listToUse[k];

                if (!Owner.Tiles.ContainsKey(neighbour))
                    continue;

                foreach (Unit unit in list) {
                    if (unit.gridPos == neighbour)
                        skip = true;
                }

                if (!Owner.Tiles[neighbour].CompareTag("WalkableTile") || AccessableTiles.Contains(neighbour) || skip)
                    continue;

                AttackableTiles.Add(neighbour);
            }
        }
    }

    public virtual void ResetTiles() {
        AccessableTiles.Clear();
        AttackableTiles.Clear();
        TileParents.Clear();
    }

    public virtual void FindPathToTile(Vector2Int gridPos) {
        if (IsWalking)
            return;

        currentPath = null;

        if (AccessableTiles.Contains(gridPos)) {
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
        }

        if(currentPath.Count < 1) {
            FindAccessableTiles();
            IsWalking = false;
        }
    }
}