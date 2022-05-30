using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitComponents {
    public class SpeedEffect : UnitEffect {
        public override void ApplyEffect(UnitValues values) {
            values.speedValue += ValueToChange;
        }
    }
}