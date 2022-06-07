using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal Ability", menuName = "Abilities/HealAbility")]
public class HealAbilityBase : AbilityBase
{
    public int HealAmount;

    public override void WhatItDoes(Vector2Int[] pos, UnitManager[] targets) {
        foreach (var unit in targets) {
            unit.HealthComponent.Heal(HealAmount);
        }

        isDone = true;
    }

    public override string ToolTipText(List<HealthComponent> targetHealthComponents) {
        string Heals = "";

        for (int i = 0; i < targetHealthComponents.Count; i++) {
            var thisString = targetHealthComponents[i].owner.gameObject.name + "\n";

            thisString += "Heal " + HealAmount;

            if (i < targetHealthComponents.Count - 1)
                thisString += "\n \n";

            Heals += thisString;
        }

        return Heals;
    }
}