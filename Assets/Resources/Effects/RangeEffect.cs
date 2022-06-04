using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitComponents;

[CreateAssetMenu(fileName = "Range Effect", menuName = "Effects/RangeEffect")]
public class RangeEffect : UnitEffect 
{
    public override void ApplyEffect(UnitValues values) {
        values.rangeValue += ValueToChange;
    }
}