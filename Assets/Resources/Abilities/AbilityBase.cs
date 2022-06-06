using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitComponents;

//[CreateAssetMenu(fileName = "Ability")]
public abstract class AbilityBase : ScriptableObject
{
    [Header("Identifiers:")]
    public new string name;
    public Sprite Icon;

    [Header("Ability Settings, Fill which is Applicable:")]
    public bool EndsTurn;
    public bool SelectUnit;

    public abstract void WhatItDoes(UnitManager[] target);
}