using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitComponents {
    public abstract class UnitEffect : ScriptableObject {
        public int Duration;
        public int ValueToChange;

        public abstract void ApplyEffect(UnitValues values);
    }
}