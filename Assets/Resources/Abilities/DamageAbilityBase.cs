using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAbilityBase : AbilityBase
{
    public int DamageAmount;

    public override void WhatItDoes(UnitManager target, List<Vector2Int> positions) {
        target.HealthComponent.TakeDamage(DamageAmount);
    }
}