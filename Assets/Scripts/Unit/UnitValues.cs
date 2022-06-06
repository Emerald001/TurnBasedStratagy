using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitComponents {
    public class UnitValues {
        public UnitManager owner;
        public List<UnitEffect> Effects = new List<UnitEffect>();

        //Base unit values
        public int baseSpeedValue;
        public int baseInitiativeValue;
        public int baseDamageValue;
        public int baseRangeValue;
        public int baseDefenceValue;
        public int baseHealthValue;

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

            for (int i = Effects.Count - 1; i >= 0; i--) {
                Effects[i].Duration--;

                if (Effects[i].Duration < 1)
                    Effects.RemoveAt(i);
            }

            for (int i = 0; i < Effects.Count; i++) {
                Effects[i].ApplyEffect(this);
            }

            for (int i = 0; i < owner.abilities.Count; i++) {
                if (owner.abilities[i].currentCooldown > 0)
                    owner.abilities[i].currentCooldown--;
            }
        }

        public void ApplyEffect(UnitEffect effect, int valueChanged, int duration) {
            var newEffect = ScriptableObject.Instantiate(effect);
            newEffect.ValueToChange = valueChanged;
            newEffect.Duration = duration;
            newEffect.ApplyEffect(this);

            Effects.Add(newEffect);
        }
    }
}