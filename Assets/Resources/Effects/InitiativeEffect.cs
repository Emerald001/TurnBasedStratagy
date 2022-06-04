using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitComponents;

[CreateAssetMenu(fileName = "Initiative Effect", menuName = "Effects/InitiativeEffect")]
public class InitiativeEffect : UnitEffect
{
    public override void ApplyEffect(UnitValues values) {
        values.initiativeValue += ValueToChange;
    }
}