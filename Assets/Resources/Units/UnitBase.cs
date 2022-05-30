using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit")]
public class UnitBase : ScriptableObject
{
    [Header("Identefiers: ")]
    public new string name;
    public GameObject Model;
    public Sprite portrait;

    [Header("Ranged Settings: ")]
    public bool isRanged;
    public int baseRangeValue;

    [Header("Base Settings: ")]
    public int baseDamageValue;
    public int baseInitiativeValue;
    public int baseSpeedValue;

    [Header("Health Settings: ")]
    public int baseHealthValue;
    public int baseDefenceValue;

    [Header("Abilities: ")]
    public List<AbilityBase> abilities;
}