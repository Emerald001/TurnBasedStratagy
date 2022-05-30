using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitComponents {
    public abstract class UnitEffect : MonoBehaviour {
        public int Duration;
        public int ValueToChange;

        public abstract void ApplyEffect(UnitValues values);
    }
}