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
        Vector2Int CalculatedDamage = CalcDamage(Damage);
        var damPoints = Random.Range(CalculatedDamage.x, CalculatedDamage.y);

        Health -= damPoints;

        UpdateHealth();
    }

    public void Heal(int Amount) {
        Health += Amount;
        if(Health > baseHealthValue) {
            Health = baseHealthValue;
        }
    }

    public Vector2Int CalcDamage(int damage) {
        var newdamage = damage * (100f / (100f + Defence));

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

        var Owner = GetComponent<UnitManager>();
        Owner.turnManager.UnitDeath(Owner);
    }
}