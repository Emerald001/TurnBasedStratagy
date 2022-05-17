using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable 
{
    public int Health { get; set; }
    public int Defence { get; set; }
    public void TakeDamage(int Damage);
    public void Heal(int Amount);
    public void OnDeath();
}