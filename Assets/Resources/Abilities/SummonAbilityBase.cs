using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Summon Ability", menuName = "Abilities/SummonAbility")]
public class SummonAbilityBase : AbilityBase
{
    public GameObject prefab;
    public UnitBase UnitToSpawn;

    //Make when Vrij is done!

    public override void WhatItDoes(UnitManager[] target) {
        //List<Vector3> spawnpoint = new List<Vector3>();

        //foreach (var pos in positions) {
        //    spawnpoint.Add(UnitStaticFunctions.CalcWorldPos(pos));
        //}

        //var spawnedUnit = GameObject.Instantiate(prefab);
    }
}