using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealthComponent
{
    public int baseDefenceValue;
    public int baseHealthValue;

    public virtual void TakeDamage(float damage) {

    }
}
