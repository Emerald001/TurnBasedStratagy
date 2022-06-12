using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitComponents;

[CreateAssetMenu (fileName = "DefenceEffect", menuName = "Effects/DefenceEffect")]
public class DefenceEffect : UnitEffect {
    public override void ApplyEffect(UnitValues values) {
        values.defenceValue += ValueToChange;
    }
}
