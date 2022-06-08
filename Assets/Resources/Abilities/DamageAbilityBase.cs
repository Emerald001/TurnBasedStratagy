using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage Ability", menuName = "Abilities/DamageAbility")]
public class DamageAbilityBase : AbilityBase
{
    public int DamageAmount;

    public override void WhatItDoes(Vector2Int[] pos, UnitManager[] targets) {
        foreach (var unit in targets) {
            unit.HealthComponent.TakeDamage(DamageAmount);
            unit.UnitAnimator.HitEnemy(unit);
        }

        isDone = true;
    }

    public override string ToolTipText(List<HealthComponent> targetHealthComponents) {
        string kills = "";

        for (int i = 0; i < targetHealthComponents.Count; i++) {
            var thisString = targetHealthComponents[i].Owner.gameObject.name + "\n";
            Vector2Int minmax = targetHealthComponents[i].CalcDamage(DamageAmount);

            thisString += "Damage " + minmax.x + "-" + minmax.y + "\n";

            var MinKills = minmax.x > targetHealthComponents[i].Health ? 1 : 0;
            var MaxKills = minmax.y < targetHealthComponents[i].Health ? 0 : 1;

            if (MinKills != MaxKills)
                thisString += "Kills " + MinKills + "-" + MaxKills;
            else
                thisString += "Kills " + MinKills;

            if (i < targetHealthComponents.Count - 1)
                thisString += "\n \n";

            kills += thisString;
        }

        if(kills != "")
            return kills;

        return null;
    }
}