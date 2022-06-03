using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal Ability", menuName = "Abilities/HealAbility")]
public class HealAbilityBase : AbilityBase
{
    public int HealAmount;

    public override void WhatItDoes(UnitManager target, params Vector2Int[] positions) {
        target.HealthComponent.Heal(HealAmount);
    }
}