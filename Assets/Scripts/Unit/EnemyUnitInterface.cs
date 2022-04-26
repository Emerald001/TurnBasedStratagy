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
        //Picks the action Properly

        if (AttackableTiles.Count != 0)
            PickedTile(AttackableTiles[Random.Range(0, AttackableTiles.Count)]);
        else if (AccessableTiles.Count != 0)
            PickedTile(AccessableTiles[Random.Range(0, AccessableTiles.Count)]);
    }

    public float WaitTime() {
        return timer -= Time.deltaTime;
    }

    public override void FindTiles() {
        base.FindTiles();
    }
}