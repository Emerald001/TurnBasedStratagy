using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit {

    public LineRenderer line;

    public override void OnEnter() {
        base.OnEnter();
        line = GetComponent<LineRenderer>();

        FindAccessableTiles();
        this.gameObject.GetComponentInChildren<Renderer>().material = Owner.ActiveUnitColor;
    }

    public override void OnUpdate() {
        base.OnUpdate();

        if (currentPath != null && currentPath.Count > 0 && !IsWalking)
            DrawPath();
        
        if (AttackableTiles.Contains(Owner.blackBoard.CurrentHover)) {
            float smallestDistance = Mathf.Infinity;
            Vector2Int closestTile = Vector2Int.zero;

            var currentPos = Owner.blackBoard.CurrentHover;
            Vector2Int[] listToUse;

            if (currentPos.y % 2 != 0)
                listToUse = unevenNeighbours;
            else
                listToUse = evenNeighbours;

            for (int k = 0; k < 6; k++) {
                var neighbour = currentPos + listToUse[k];

                if (!AccessableTiles.Contains(neighbour))
                    continue;

                if(Vector3.Distance(Owner.makeGrid.CalcWorldPos(neighbour), Owner.blackBoard.HoverPoint) < smallestDistance) {
                    smallestDistance = Vector3.Distance(Owner.makeGrid.CalcWorldPos(neighbour), Owner.blackBoard.HoverPoint);
                    closestTile = neighbour;
                }
            }

            if(closestTile != Vector2Int.zero) {
                FindPathToTile(closestTile);
                currentPath.Add(currentPos);
            }
        }
        else if(AccessableTiles.Contains(Owner.blackBoard.CurrentHover)) {
            FindPathToTile(Owner.blackBoard.CurrentHover);
        }
        else if(!IsWalking) {
            currentPath = null;
            line.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            if (currentPath != null && currentPath.Count > 0) {
                currentPath.RemoveAt(0);
                IsWalking = true;
                line.enabled = false;
                ResetTiles();
            }
        }

        if (IsWalking)
            MoveToTile();
    }

    public override void OnExit() {
        ResetTiles();
        line.enabled = false;
        this.gameObject.GetComponentInChildren<Renderer>().material = Owner.NormalUnitColor;
    }

    public override void ResetTiles() {
        for (int i = 0; i < AccessableTiles.Count; i++) {
            if (Owner.Tiles[AccessableTiles[i]].GetComponent<Hex>().GivenColor == Owner.WalkableTileColor) {
                Owner.Tiles[AccessableTiles[i]].GetComponent<Hex>().GivenColor = null;
            }
            Owner.Tiles[AccessableTiles[i]].GetComponent<Hex>().ResetColor();
        }
        for (int i = 0; i < AttackableTiles.Count; i++) {
            if (Owner.Tiles[AttackableTiles[i]].GetComponent<Hex>().GivenColor == Owner.AttackableTileColor) {
                Owner.Tiles[AttackableTiles[i]].GetComponent<Hex>().GivenColor = null;
            }
            Owner.Tiles[AttackableTiles[i]].GetComponent<Hex>().ResetColor();
        }

        base.ResetTiles();
    }

    public override void FindPathToTile(Vector2Int gridPos) {
        base.FindPathToTile(gridPos);
        if (!AccessableTiles.Contains(gridPos))
            line.enabled = false;
    }

    public override void FindAccessableTiles() {
        base.FindAccessableTiles();
        ChangeHexColor(AccessableTiles, Owner.WalkableTileColor);

        base.FindMeleeAttackableTiles(Owner.enemiesInPlay);
        ChangeHexColor(AttackableTiles, Owner.AttackableTileColor);
    }

    public void ChangeHexColor(List<Vector2Int> list, Material color) {
        for (int i = 0; i < list.Count; i++) {
            Owner.Tiles[list[i]].GetComponent<Hex>().GivenColor = color;
            Owner.Tiles[list[i]].GetComponent<Hex>().SetColor(color);
        }
    }

    private void DrawPath() {
        line.enabled = true;

        if (currentPath != null && currentPath.Count > 0) {
            line.positionCount = currentPath.Count;
            for (int i = 0; i < currentPath.Count; i++) {
                line.SetPosition(i, Owner.makeGrid.CalcWorldPos(currentPath[i]));
            }
        }
    }
}