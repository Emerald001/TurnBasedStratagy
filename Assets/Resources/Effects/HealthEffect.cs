using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitComponents;

[CreateAssetMenu(fileName = "Health Effect", menuName = "Effects/HealthEffect")]
public class HealthEffect : UnitEffect {
    public override void ApplyEffect(UnitValues values) {
        if (ValueToChange < 0) {
            values.owner.HealthComponent.TakeDamage(-ValueToChange);
            values.owner.UnitAnimator.HitAnim();
        }
        else
            values.owner.HealthComponent.Heal(ValueToChange);
    }
}