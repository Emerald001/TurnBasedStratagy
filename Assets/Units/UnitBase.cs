using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit")]
public class UnitBase : ScriptableObject
{
    public Vector2Int gridPos;

    public bool isRanged;
    public bool spawnLeft;

    public int baseDamageValue;
    public int baseInitiativeValue;
    public int baseSpeedValue;
    public int baseRangeValue;

    public int baseHealthValue;
    public int baseDefenceValue;
}