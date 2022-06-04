using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage Ability", menuName = "Abilities/DamageAbility")]
public class DamageAbilityBase : AbilityBase
{
    public int DamageAmount;

    public override void WhatItDoes(UnitManager[] targets) {
        foreach (var unit in targets) {
            unit.HealthComponent.TakeDamage(DamageAmount);
        }
    }
}