using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitComponents;

public class EnemyUnitInterface : UnitManager {
    public float timer;

    public override void OnEnter() {
        base.OnEnter();
        timer = .5f;
    }

    public override void OnExit() {
        base.OnExit();
    }

    public override void OnUpdate() {
        base.OnUpdate();

        if (WaitTime() > 0) {
            return;
        }

        PickAction();
    }

    private void PickAction() {
        if (AttackableTiles.Count != 0) {
            UnitManager lastEnemy = null;

            for (int i = 0; i < AttackableTiles.Count; i++) {
                if (lastEnemy == null) {
                    lastEnemy = EnemyPositions[AttackableTiles[i]].GetComponent<UnitManager>();
                    continue;
                }

                if (EnemyPositions[AttackableTiles[i]].HealthComponent.Health < lastEnemy.HealthComponent.Health)
                    lastEnemy = EnemyPositions[AttackableTiles[i]].GetComponent<UnitManager>();
            }

            Vector2Int pickedTile = lastEnemy.gridPos;
            PickedTile(pickedTile, defineAttackableTiles.GetClosestTile(gridPos, pickedTile, Vector3.zero, AccessableTiles));
        }
        else if (AccessableTiles.Count != 0) {
            Vector2Int pickedTile = new Vector2Int(100, 100);

            for (int i = 0; i < AccessableTiles.Count; i++) {
                if (AccessableTiles[i].x < pickedTile.x)
                    pickedTile = AccessableTiles[i];
            }
            if(values.baseRangeValue > 1) {
                if(AccessableTiles.Contains(pickedTile + new Vector2Int(1, 0)))
                    pickedTile.x += 1;
            }

            PickedTile(pickedTile, Vector2Int.zero);
        }
    }

    public float WaitTime() {
        return timer -= Time.deltaTime;
    }

    public override void FindTiles() {
        base.FindTiles();
    }

    public override void ResetTiles() {
        base.ResetTiles();
    }
}