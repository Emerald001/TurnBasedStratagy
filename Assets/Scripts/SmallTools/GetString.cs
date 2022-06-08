using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitComponents;

public static class GetString {

    public static string GetName(UnitManager unit) {
        return unit.name;
    }

    

    public static List<AbilityBase> GetAbilities(UnitManager unit) {
        return unit.abilities;
    }

    public static List<UnitEffect> GetEffects(UnitValues unit) {
        return unit.Effects;
    }
}