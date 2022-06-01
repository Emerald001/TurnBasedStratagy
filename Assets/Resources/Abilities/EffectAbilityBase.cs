using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitComponents;

[CreateAssetMenu(fileName = "Effect Ability")]
public class EffectAbilityBase : AbilityBase {
    public int valueChanged;
    public UnitEffect effect;

    public override void WhatItDoes(UnitManager target, params Vector2Int[] positions) {

    }
}