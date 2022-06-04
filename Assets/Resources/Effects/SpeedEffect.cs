using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitComponents;

[CreateAssetMenu(fileName = "Speed Effect", menuName = "Effects/SpeedEffect")]
public class SpeedEffect : UnitEffect {
    public override void ApplyEffect(UnitValues values) {
        values.speedValue += ValueToChange;
    }
}