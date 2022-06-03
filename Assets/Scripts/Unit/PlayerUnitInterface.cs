using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitInterface : UnitManager {

    [HideInInspector] public LineRenderer line;

    private Vector2Int lastHoverPos = Vector2Int.zero;

    public override void OnEnter() {
        base.OnEnter();
        line = GetComponent<LineRenderer>();

        turnManager.UIManager.ActivateButtons();
        turnManager.UIManager.SetAbilities(abilities, this);
    }

    public override void OnUpdate() {
        if(line != null)
            CreatePathForLine();
        
        base.OnUpdate();

        //if (pickedAbility) {
        //    return;
        //}

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            PickedTile(MouseValues.HoverTileGridPos, defineAttackableTiles.GetClosestTile(gridPos, MouseValues.HoverTileGridPos, MouseValues.HoverPointPos, AccessableTiles));
            Tooltip.HideTooltip_Static();
        }

        if (AttackableTiles.Contains(MouseValues.HoverTileGridPos)) {
            var list = new List<HealthComponent>();
            list.Add(EnemyPositions[MouseValues.HoverTileGridPos].GetComponent<HealthComponent>());
            Tooltip.ShowTooltip_Static(GetEnemyInfo(list));
            lastHoverPos = MouseValues.HoverTileGridPos;
        }
        if (lastHoverPos != MouseValues.HoverTileGridPos && lastHoverPos != Vector2Int.zero) {
            Tooltip.HideTooltip_Static();
            lastHoverPos = Vector2Int.zero;
        }
    }

    public override void OnExit() {
        base.OnExit();

        turnManager.UIManager.DeactivateButtons();
        Tooltip.HideTooltip_Static();
        line.enabled = false;
    }

    public override void ResetTiles() {
        ResetHexColor(AccessableTiles);
        ResetHexColor(AttackableTiles);

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
            var hex = turnManager.Tiles[list[i]].GetComponent<Hex>();
            hex.GivenColor = color;
            hex.SetColor(color);
        }
    }
    public void ResetHexColor(List<Vector2Int> list) {
        for (int i = 0; i < list.Count; i++) {
            var hex = turnManager.Tiles[list[i]].GetComponent<Hex>();
            hex.GivenColor = null;
            hex.ResetColor();
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

    private string GetEnemyInfo(List<HealthComponent> enemyHealthComponents) {
        string kills = "";

        for (int i = 0; i < enemyHealthComponents.Count; i++) {
            var thisString = enemyHealthComponents[i].gameObject.name + "\n";
            Vector2Int minmax = enemyHealthComponents[i].CalcDamage(values.damageValue);

            thisString += "Damage " + minmax.x + "-" + minmax.y + "\n";

            var MinKills = minmax.x > enemyHealthComponents[i].Health ? 1 : 0;
            var MaxKills = minmax.y < enemyHealthComponents[i].Health ? 0 : 1;

            if(MinKills != MaxKills)
                thisString += "Kills " + MinKills + "-" + MaxKills;
            else
                thisString += "Kills " + MinKills;

            if (i < enemyHealthComponents.Count - 1)
                thisString += "\n";

            kills += thisString;
        }

        return kills;
    }
}