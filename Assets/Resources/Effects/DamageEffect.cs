using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitComponents;

[CreateAssetMenu(fileName = "Damage Effect", menuName = "Effects/DamageEffect")]
public class DamageEffect : UnitEffect
{
    public override void ApplyEffect(UnitValues values) {
        values.damageValue += ValueToChange;
    }
}