using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitInterface : UnitManager {

    [HideInInspector] public LineRenderer line;

    public override void OnEnter() {
        base.OnEnter();
        line = GetComponent<LineRenderer>();
    }

    public override void OnUpdate() {
        base.OnUpdate();

        if (!AccessableTiles.Contains(gridPos))
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

        base.ResetTiles();
    }
}