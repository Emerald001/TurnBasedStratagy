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
        private List<int> Values;
        public int speedValue;
        public int initiativeValue;
        public int damageValue;
        public int rangeValue;

        public void SetValues() {
            //Values are defined here at the start of a turn, to be made nice and open to call whenever

            speedValue = baseSpeedValue;
            initiativeValue = baseInitiativeValue;
            damageValue = baseDamageValue;
            rangeValue = baseRangeValue;

            for (int i = 0; i < Effects.Count; i++) {
                ApplyEffect(Effects[i], Values);
            }
        }

        public void ApplyEffect(UnitEffect effect, List<int> values) {

        }

        public void TurnUpdate() {
            for (int i = 0; i < Effects.Count; i++) {
                Effects[i].Duration--;
            }
        }
    }
}