using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitComponents {
    public abstract class UnitEffect : ScriptableObject {
        [HideInInspector] public int Duration;
        [HideInInspector] public int ValueToChange;

        public abstract void ApplyEffect(UnitValues values);
    }
}