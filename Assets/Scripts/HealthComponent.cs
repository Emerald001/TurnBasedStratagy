using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamagable
{
    public UnitManager Owner;

    public int baseDefenceValue;
    public int baseHealthValue;

    public int Health { get; set; }
    public int Defence { get; set; }

    public void TakeDamage(int Damage) {
        var damPoints = 0;

        if (Defence < Damage) {
            damPoints = (Damage * 2) / Mathf.RoundToInt(Defence);
        }
        else {
            damPoints = Damage - Mathf.RoundToInt(Defence);
        }

        Health -= Damage;

        CheckHealth();
    }

    public void CheckHealth() {
        if(Health < 1) {
            OnDeath();
        }
    }

    public void OnDeath() {
        Owner.turnManager.AllUnitsInPlay.Remove(Owner);
    }
}