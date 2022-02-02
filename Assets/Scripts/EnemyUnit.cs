using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit {
    public float timer;

    public override void OnEnter() {
        base.OnEnter();
        timer = .5f;

        FindAccessableTiles();
    }

    public override void OnExit() {
        ResetTiles();
    }

    public override void OnUpdate() {
        base.OnUpdate();

        if (WaitTime() > 0) {
            return;
        }

        if(AccessableTiles.Count != 0)
            FindPathToTile(AccessableTiles[Random.Range(0, AccessableTiles.Count - 1)]);

        if (currentPath != null && currentPath.Count > 0) {
            //currentPath.RemoveAt(0);
            IsWalking = true;
            ResetTiles();
        }

        if (IsWalking)
            MoveToTile();
    }

    public float WaitTime() {
        return timer -= Time.deltaTime;
    }

    public override void FindAccessableTiles() {
        base.FindAccessableTiles();
        base.FindMeleeAttackableTiles(Owner.unitsInPlay);
    }
}
