using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitComponents;

public class PlayerUnitInterface : UnitManager {

    [HideInInspector] public LineRenderer line;

    private Vector2Int lastHoverPos = Vector2Int.zero;
    private List<Vector2Int> lastAbilityTiles = new List<Vector2Int>();
    private List<Vector2Int> lastHighlightedTiles = new List<Vector2Int>();

    public override void OnEnter() {
        line = GetComponent<LineRenderer>();
        turnManager.UIManager.ActivateButtons();

        base.OnEnter();
    }

    public override void OnUpdate() {
        if (line != null)
            CreatePathForLine();

        base.OnUpdate();

        if (pickedAbility) {
            RunAbility();
            return;
        }

        RunAttack();
    }

    private void RunAttack() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            PickedTile(MouseValues.HoverTileGridPos, defineAttackableTiles.GetClosestTile(gridPos, MouseValues.HoverTileGridPos, MouseValues.HoverPointPos, AccessableTiles));
            Tooltip.HideTooltip_Static();
        }

        if (AttackableTiles.Contains(MouseValues.HoverTileGridPos)) {
            var list = new List<HealthComponent>();
            list.Add(EnemyPositions[MouseValues.HoverTileGridPos].HealthComponent);
            Tooltip.ShowTooltip_Static(GetEnemyInfo(list, values.damageValue));
            lastHoverPos = MouseValues.HoverTileGridPos;
        }
        if (lastHoverPos != MouseValues.HoverTileGridPos && lastHoverPos != Vector2Int.zero) {
            Tooltip.HideTooltip_Static();
            lastHoverPos = Vector2Int.zero;
        }
    }

    private void RunAbility() {
        List<Vector2Int> highlightedPositions = new List<Vector2Int>();

        foreach (var pos in lastHighlightedTiles) {
            if (!highlightedPositions.Contains(pos)) {
                var hex = turnManager.Tiles[pos].GetComponent<Hex>();
                hex.ResetColor();
            }
        }

        if (pickedAbility.HitDiameter >= 1 && pickedAbility.Tiles.Contains(MouseValues.HoverTileGridPos)) {
            highlightedPositions = DefineMultipleTiles.GetTiles(MouseValues.HoverTileGridPos, pickedAbility.HitDiameter, turnManager.Tiles);
            foreach (var pos in highlightedPositions) {
                var hex = turnManager.Tiles[pos].GetComponent<Hex>();
                hex.SetColor(turnManager.SelectedTileColor);
            }
            lastHighlightedTiles = highlightedPositions;
        }
        else {
            highlightedPositions.Add(MouseValues.HoverTileGridPos);
        }

        if (pickedAbility.Tiles.Contains(MouseValues.HoverTileGridPos)) {
            var list = new List<HealthComponent>();

            foreach (var target in highlightedPositions) {
                if(pickedAbility.AbilityApplicable.ContainsKey(target))
                    list.Add(pickedAbility.AbilityApplicable[target].HealthComponent);
            }
            if(pickedAbility.ToolTipText(list) == null)
                Tooltip.HideTooltip_Static();
            else
                Tooltip.ShowTooltip_Static(pickedAbility.ToolTipText(list));

            lastHoverPos = MouseValues.HoverTileGridPos;
        }
        if (lastHoverPos != MouseValues.HoverTileGridPos && lastHoverPos != Vector2Int.zero) {
            Tooltip.HideTooltip_Static();
            lastHoverPos = Vector2Int.zero;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            if(pickedAbility.Definer != null)
                pickedAbility.PickedTile(highlightedPositions.ToArray(), pickedAbility.Definer.GetClosestTile(gridPos, MouseValues.HoverTileGridPos, MouseValues.HoverPointPos, AccessableTiles));
            else
                pickedAbility.PickedTile(highlightedPositions.ToArray(), gridPos);
            Tooltip.HideTooltip_Static();
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

        if (lastAbilityTiles.Count > 0)
            ResetHexColor(lastAbilityTiles);
        lastAbilityTiles.Clear();

        foreach (var pos in lastHighlightedTiles) {
            var hex = turnManager.Tiles[pos].GetComponent<Hex>();
            hex.ResetColor();
        }

        turnManager.Tiles[gridPos].GetComponent<Hex>().GivenColor = null;
        turnManager.Tiles[gridPos].GetComponent<Hex>().ResetColor();

        base.ResetTiles();
    }

    public override void FindTiles() {
        //Hella cursed, shall be made better soon
        turnManager.UIManager.SetAbilities(abilities, this);

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

        if (pickedAbility != null) {
            if (pickedAbility.Tiles.Contains(endPos) && pickedAbility.Definer != null) {
                CurrentPath = pathfinding.FindPathToTile(gridPos, pickedAbility.Definer.GetClosestTile(gridPos, endPos, MouseValues.HoverPointPos, AccessableTiles), TileParents);
                CurrentPath.Add(MouseValues.HoverTileGridPos);
            }
            else {
                line.enabled = false;
                return;
            }
        }
        else if (AccessableTiles.Contains(endPos))
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
                if (i == 0) {
                    line.SetPosition(0, UnitStaticFunctions.CalcWorldPos(gridPos));
                    continue;
                }
                line.SetPosition(i, UnitStaticFunctions.CalcWorldPos(CurrentPath[i - 1]));
            }
        }
    }

    public override void SelectAbility(int index) {
        base.SelectAbility(index);

        line.enabled = false;

        if (pickedAbility != null) {
            if(pickedAbility.Ranged || pickedAbility.DropAnywhere || pickedAbility.DropOnEmptyTile)
                ResetHexColor(AccessableTiles);
            ResetHexColor(AttackableTiles);

            if(lastAbilityTiles.Count > 0)
                ResetHexColor(lastAbilityTiles);

            ChangeHexColor(pickedAbility.Tiles, turnManager.AttackableTileColor);

            foreach (var tile in pickedAbility.Tiles) {
                lastAbilityTiles.Add(tile);
            }
        }
        if(pickedAbility == null) {
            ResetHexColor(lastAbilityTiles);
            lastAbilityTiles.Clear();

            ChangeHexColor(AccessableTiles, turnManager.WalkableTileColor);
            ChangeHexColor(AttackableTiles, turnManager.AttackableTileColor);
        }
    }

    private string GetEnemyInfo(List<HealthComponent> enemyHealthComponents, int DamageValue) {
        string kills = "";

        for (int i = 0; i < enemyHealthComponents.Count; i++) {
            var thisString = enemyHealthComponents[i].owner.gameObject.name + "\n";
            Vector2Int minmax = enemyHealthComponents[i].CalcDamage(DamageValue);

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