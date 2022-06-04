using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitComponents;

[CreateAssetMenu(fileName = "Effect Ability", menuName = "Abilities/EffectAbility")]
public class EffectAbilityBase : AbilityBase {
    public int valueChanged;
    public UnitEffect effect;

    public override void WhatItDoes(UnitManager[] targets) {
        foreach (UnitManager unit in targets)
            unit.AddEffect(effect);
    }
}