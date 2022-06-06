using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthComponent : IDamagable
{
    public UnitManager owner;

    private Slider HealthBar;

    public int Health { get; set; }

    public void OnEnter() {
        HealthBar = owner.GetComponentInChildren<Slider>();
        HealthBar.maxValue = owner.values.baseHealthValue;
        Health = owner.values.baseHealthValue;
    }

    public void TakeDamage(int Damage) {
        Vector2Int CalculatedDamage = CalcDamage(Damage);
        var damPoints = Random.Range(CalculatedDamage.x, CalculatedDamage.y);

        Health -= damPoints;

        UpdateHealth();
    }

    public void Heal(int Amount) {
        Health += Amount;
        if(Health > Health) {
            Health = owner.values.baseHealthValue;
        }
    }

    public Vector2Int CalcDamage(int damage) {
        var newdamage = damage * (100f / (100f + owner.values.defenceValue));

        var difference = damage - newdamage;

        int minDamage = Mathf.RoundToInt(newdamage - difference * 2);
        int maxDamage = Mathf.RoundToInt(damage - difference / 2);

        return new Vector2Int(minDamage, maxDamage);
    }

    private void UpdateHealth() {
        HealthBar.value = Health;

        if(Health < 1) {
            OnDeath();
        }
    }

    public void OnDeath() {
        HealthBar.transform.parent.gameObject.SetActive(false);

        var Owner = owner.GetComponent<UnitManager>();
        Owner.turnManager.UnitDeath(Owner);
    }
}