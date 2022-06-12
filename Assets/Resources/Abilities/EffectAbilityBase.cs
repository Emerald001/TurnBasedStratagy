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
    public bool DoHitAnimation;

    public override void WhatItDoes(Vector2Int[] pos, UnitManager[] targets) {
        foreach (var unit in targets) {
            unit.AddEffect(effect, valueChanged, duration, EffectDesciption, EffectIcon);

            if(DoHitAnimation)
                unit.UnitAnimator.HitEnemy(unit);
        }

        isDone = true;
    }

    public override string ToolTipText(List<HealthComponent> targetHealthComponents) {
        string kills = "";

        for (int i = 0; i < targetHealthComponents.Count; i++) {
            var thisString = targetHealthComponents[i].Owner.gameObject.name;

            if (i < targetHealthComponents.Count - 1)
                thisString += "\n";

            kills += thisString;
        }

        if (kills != "")
            return kills;

        return null;
    }
}