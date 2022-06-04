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
        public int baseDefenceValue;

        //Current unit values
        public int speedValue;
        public int initiativeValue;
        public int damageValue;
        public int rangeValue;
        public int defenceValue;

        public void SetValues() {
            //Values are defined here at the start of a turn, to be made nice and open to call whenever

            speedValue = baseSpeedValue;
            initiativeValue = baseInitiativeValue;
            damageValue = baseDamageValue;
            rangeValue = baseRangeValue;
            defenceValue = baseDefenceValue;

            for (int i = 0; i < Effects.Count; i++) {
                Effects[i].ApplyEffect(this);
            }

            foreach (var effect in Effects) {
                effect.Duration--;

                if (effect.Duration < 1)
                    Effects.Remove(effect);
            }
        }

        public void ApplyEffect(UnitEffect effect, int valueChanged, int duration) {
            var newEffect = ScriptableObject.Instantiate(effect);
            newEffect.ApplyEffect(this);
            newEffect.ValueToChange = valueChanged;
            newEffect.Duration = duration;

            Effects.Add(newEffect);
        }
    }
}