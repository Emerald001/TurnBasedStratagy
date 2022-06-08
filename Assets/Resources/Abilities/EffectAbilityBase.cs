using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitComponents;

[CreateAssetMenu(fileName = "Effect Ability", menuName = "Abilities/EffectAbility")]
public class EffectAbilityBase : AbilityBase {
    public int valueChanged;
    public int duration;
    public UnitEffect effect;

    public string EffectDesciption;
    public Sprite EffectIcon;

    public override void WhatItDoes(Vector2Int[] pos, UnitManager[] targets) {
        foreach (var unit in targets) {
            unit.AddEffect(effect, valueChanged, duration, EffectDesciption, EffectIcon);
        }

        isDone = true;
    }

    public override string ToolTipText(List<HealthComponent> targetHealthComponents) {
        return null;
    }
}