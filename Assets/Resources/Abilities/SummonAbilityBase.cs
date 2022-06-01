using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonAbilityBase : AbilityBase
{
    public GameObject prefab;
    public UnitBase UnitToSpawn;

    public override void WhatItDoes(UnitManager target, Vector2Int[] positions) {
        List<Vector3> spawnpoint = new List<Vector3>();

        foreach(var pos in positions) {
            spawnpoint.Add(UnitStaticFunctions.CalcWorldPos(pos));
        }

        var spawnedUnit = GameObject.Instantiate(prefab);
    }
}