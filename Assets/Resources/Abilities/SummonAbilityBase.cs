using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitComponents;

[CreateAssetMenu(fileName = "Summon Ability", menuName = "Abilities/SummonAbility")]
public class SummonAbilityBase : AbilityBase
{
    public GameObject prefab;
    public UnitBase UnitToSpawn;
    public override void WhatItDoes(Vector2Int[] pos, UnitManager[] targets) {
        foreach (var position in pos) {
            UnitSpawn.SpawnUnits(Runner.turnManager, prefab, UnitToSpawn, position, Runner.OwnList, Runner.EnemyList, Runner.turnManager.transform);
        }

        isDone = true;
    }

    public override string ToolTipText(List<HealthComponent> targetHealthComponents) {
        return null;
    }
}