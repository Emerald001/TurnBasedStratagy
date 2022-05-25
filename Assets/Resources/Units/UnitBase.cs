using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit")]
public class UnitBase : ScriptableObject
{
    public bool isRanged;

    public int baseDamageValue;
    public int baseInitiativeValue;
    public int baseSpeedValue;
    public int baseRangeValue;

    public int baseHealthValue;
    public int baseDefenceValue;
}