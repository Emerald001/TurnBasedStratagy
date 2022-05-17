using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour, IDamagable
{
    public int baseDefenceValue;
    public int baseHealthValue;

    private Slider HealthBar;

    public int Health { get; set; }
    public int Defence { get; set; }

    private void Start() {
        HealthBar = GetComponentInChildren<Slider>();
        HealthBar.maxValue = baseHealthValue;
    }

    public void TakeDamage(int Damage) {
        var damPoints = 0;

        if (Defence < Damage) {
            damPoints = (Damage * 2) / Mathf.RoundToInt(Defence);
        }
        else {
            damPoints = Damage - Mathf.RoundToInt(Defence);
        }

        Health -= Damage;

        UpdateHealth();
    }

    public void Heal(int Amount) {
        Health += Amount;
        if(Health > baseHealthValue) {
            Health = baseHealthValue;
        }
    }

    private void UpdateHealth() {
        HealthBar.value = Health;

        if(Health < 1) {
            OnDeath();
        }
    }

    public void OnDeath() {
        var Owner = GetComponent<UnitManager>();
        Owner.turnManager.UnitDeath(Owner);
    }
}