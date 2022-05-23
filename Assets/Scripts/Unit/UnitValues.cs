using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitComponents {
    public class UnitValues {
        public List<UnitEffect> Effects = new List<UnitEffect>();

        //Base unit values
        public int baseSpeedValue;
        public int baseInitiativeValue;
        public int baseDamageValue;
        public int baseRangeValue;

        //Current unit values
        public int speedValue;
        public int initiativeValue;
        public int damageValue;
        public int rangeValue;

        public void SetValues() {
            //Values are defined here, to be made nice and open to call whenever

            speedValue = baseSpeedValue;
            initiativeValue = baseInitiativeValue;
            damageValue = baseDamageValue;
            rangeValue = baseRangeValue;

            for (int i = 0; i < Effects.Count; i++) {
                Effects[i].ApplyEffect(this);
            }
        }

        public void TurnUpdate() {
            for (int i = 0; i < Effects.Count; i++) {

            }
        }
    }
}