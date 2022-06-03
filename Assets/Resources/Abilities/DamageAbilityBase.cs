using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage Ability", menuName = "Abilities/DamageAbility")]
public class DamageAbilityBase : AbilityBase
{
    public int DamageAmount;

    public override void WhatItDoes(UnitManager target, params Vector2Int[] positions) {
        target.HealthComponent.TakeDamage(DamageAmount);
    }
}