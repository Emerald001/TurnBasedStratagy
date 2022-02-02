using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public int baseDefenceValue;
    public int baseHealthValue;

    public int HealthValue;
    public int DefenceValue;

    public virtual void TakeDamage(int damage) {
        var damPoints = 0;

        if (DefenceValue < damage) {
            damPoints = (damage * 2) / DefenceValue;
        }
        else {
            damPoints = damage - DefenceValue;
        }

        HealthValue -= damage;

        CheckHealth();
    }

    public virtual void CheckHealth() {
        if(HealthValue < 1) {
            Destroy(gameObject);
        }
    }
}