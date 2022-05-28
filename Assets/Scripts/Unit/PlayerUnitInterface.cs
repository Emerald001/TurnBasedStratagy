using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitInterface : UnitManager {

    [HideInInspector] public LineRenderer line;

    private Vector2Int lastHoverPos = Vector2Int.zero;

    public override void OnEnter() {
        base.OnEnter();
        line = GetComponent<LineRenderer>();
    }

    public override void OnUpdate() {
        if(line != null)
            CreatePathForLine();
        
        base.OnUpdate();

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            PickedTile(MouseValues.HoverTileGridPos, defineAttackableTiles.GetClosestTile(gridPos, MouseValues.HoverTileGridPos, MouseValues.HoverPointPos, AccessableTiles));
            Tooltip.HideTooltip_Static();
        }

        if (AttackableTiles.Contains(MouseValues.HoverTileGridPos)) {
            Tooltip.ShowTooltip_Static(GetEnemyInfo(EnemyPositions[MouseValues.HoverTileGridPos].GetComponent<HealthComponent>()));
            lastHoverPos = MouseValues.HoverTileGridPos;
        }
        if (lastHoverPos != MouseValues.HoverTileGridPos && lastHoverPos != Vector2Int.zero) {
            Tooltip.HideTooltip_Static();
            lastHoverPos = Vector2Int.zero;
        }
    }

    public override void OnExit() {
        base.OnExit();

        Tooltip.HideTooltip_Static();
        line.enabled = false;
    }

    public override void ResetTiles() {
        for (int i = 0; i < AccessableTiles.Count; i++) {
            if (turnManager.Tiles[AccessableTiles[i]].GetComponent<Hex>().GivenColor == turnManager.WalkableTileColor) {
                turnManager.Tiles[AccessableTiles[i]].GetComponent<Hex>().GivenColor = null;
            }
            turnManager.Tiles[AccessableTiles[i]].GetComponent<Hex>().ResetColor();
        }
        for (int i = 0; i < AttackableTiles.Count; i++) {
            if (turnManager.Tiles[AttackableTiles[i]].GetComponent<Hex>().GivenColor == turnManager.AttackableTileColor) {
                turnManager.Tiles[AttackableTiles[i]].GetComponent<Hex>().GivenColor = null;
            }
            turnManager.Tiles[AttackableTiles[i]].GetComponent<Hex>().ResetColor();
        }

        turnManager.Tiles[gridPos].GetComponent<Hex>().GivenColor = null;
        turnManager.Tiles[gridPos].GetComponent<Hex>().ResetColor();

        base.ResetTiles();
    }

    public override void FindTiles() {
        base.FindTiles();

        ChangeHexColor(AccessableTiles, turnManager.WalkableTileColor);
        ChangeHexColor(AttackableTiles, turnManager.AttackableTileColor);

        turnManager.Tiles[gridPos].GetComponent<Hex>().GivenColor = turnManager.ActiveUnitTileColor;
        turnManager.Tiles[gridPos].GetComponent<Hex>().SetColor(turnManager.ActiveUnitTileColor);
    }

    public void ChangeHexColor(List<Vector2Int> list, Material color) {
        for (int i = 0; i < list.Count; i++) {
            turnManager.Tiles[list[i]].GetComponent<Hex>().GivenColor = color;
            turnManager.Tiles[list[i]].GetComponent<Hex>().SetColor(color);
        }
    }

    private void CreatePathForLine() {
        var endPos = MouseValues.HoverTileGridPos;

        if (AccessableTiles.Contains(endPos))
            CurrentPath = pathfinding.FindPathToTile(gridPos, endPos, TileParents);
        else if (AttackableTiles.Contains(endPos)) {
            CurrentPath = pathfinding.FindPathToTile(gridPos, defineAttackableTiles.GetClosestTile(gridPos, endPos, MouseValues.HoverPointPos, AccessableTiles), TileParents);
            CurrentPath.Add(MouseValues.HoverTileGridPos);
        }
        else {
            line.enabled = false;
            return;
        }

        DrawPathWithLine();
    }
    
    private void DrawPathWithLine() {
        line.enabled = true;

        if (CurrentPath != null && CurrentPath.Count > 0) {
            line.positionCount = CurrentPath.Count + 1;
            for (int i = 0; i < CurrentPath.Count + 1; i++) {
                if(i == 0) {
                    line.SetPosition(0, UnitStaticFunctions.CalcWorldPos(gridPos));
                    continue;
                }
                line.SetPosition(i, UnitStaticFunctions.CalcWorldPos(CurrentPath[i - 1]));
            }
        }
    }

    private string GetEnemyInfo(HealthComponent enemyHealthComponent) {
        Vector2Int minmax = enemyHealthComponent.CalcDamage(values.damageValue);

        string minMaxDam = "Damage " + minmax.x + "-" + minmax.y;

        var MinKills = minmax.x > enemyHealthComponent.Health ? 1 : 0;
        var MaxKills = minmax.y < enemyHealthComponent.Health ? 0 : 1;

        string kills = "";
        if(MinKills != MaxKills)
            kills = "Kills " + MinKills + "-" + MaxKills;
        else
            kills = "Kills " + MinKills;

        return minMaxDam + "\n" + kills;
    }
}