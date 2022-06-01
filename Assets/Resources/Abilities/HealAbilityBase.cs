using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAbilityBase : AbilityBase
{
    public int HealAmount;

    public override void WhatItDoes(UnitManager target, params Vector2Int[] positions) {
        target.HealthComponent.Heal(HealAmount);
    }
}