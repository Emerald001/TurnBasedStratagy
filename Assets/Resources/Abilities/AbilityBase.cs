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
    public string Description;
    public string AnimationTrigger;
    public Sound Sound;

    [Header("Ability Settings, Fill which is Applicable:")]
    public int Cooldown;
    public bool Ranged;
    public int AbilityRange;
    public int HitDiameter;
    public bool DropAnywhere;
    public bool DropOnEmptyTile;
    public int tileRange;
    public bool SelectUnit;
    public bool targetAnything;
    public bool targetEnemy;
    public bool targetSelf;
    public bool EndsTurn;

    [HideInInspector] public int currentCooldown;

    [HideInInspector] public UnitManager Runner;
    [HideInInspector] public UnitDefineAttackableTiles Definer;
    [HideInInspector] public Dictionary<Vector2Int, UnitManager> AbilityApplicable = new Dictionary<Vector2Int, UnitManager>();
    [HideInInspector] public List<Vector2Int> Tiles = new List<Vector2Int>();

    [HideInInspector] public bool isDone;
    private Dictionary<Vector2Int, Vector2Int> PlaceHolder = new Dictionary<Vector2Int, Vector2Int>();

    public virtual void OnEnter(UnitManager unit, List<UnitManager> applicableTargets) {
        if (SelectUnit) {
            if (Ranged)
                Definer = new UnitDefineAttackableTilesRanged();
            else
                Definer = new UnitDefineAttackableTilesMelee();

            Tiles = Definer.FindAttackableTiles(unit, unit.gridPos, applicableTargets, AbilityRange, AbilityApplicable, unit.turnManager.Tiles);
        }
        else if (DropAnywhere) {
            Tiles = DefineMultipleTiles.GetTiles(unit.gridPos, tileRange, unit.turnManager.Tiles);
            foreach (var Unit in applicableTargets) {
                AbilityApplicable.Add(Unit.gridPos, Unit);
            }
        }
        else if (DropOnEmptyTile) {
            var define = new UnitDefineAccessableTiles();
            Tiles = define.FindAccessableTiles(unit, unit.gridPos, tileRange, ref PlaceHolder, unit.turnManager.Tiles);
            foreach (var Unit in applicableTargets) {
                AbilityApplicable.Add(Unit.gridPos, Unit);
            }
        }
        else {
            if (targetSelf) {
                var units = unit.GetComponents<UnitManager>();
                List<Vector2Int> positions = new List<Vector2Int>();
                positions.Add(unit.gridPos);
                unit.ActionQueue.Enqueue(new UnitAbility(unit, this, unit.gameObject, positions.ToArray(), units, null));
                unit.ResetTiles();
                OnExit();
                currentCooldown = Cooldown;
            }
            else {
                List<UnitManager> units = applicableTargets;
                List<Vector2Int> positions = new List<Vector2Int>();
                for (int i = 0; i < applicableTargets.Count; i++) {
                    positions.Add(applicableTargets[i].gridPos);
                }
                unit.ActionQueue.Enqueue(new UnitAbility(unit, this, unit.gameObject, positions.ToArray(), units.ToArray(), null));
                unit.ResetTiles();
                OnExit();
                currentCooldown = Cooldown;
            }
        }

        Runner = unit;
    }

    public virtual void OnExit() {
        Tiles.Clear();
        AbilityApplicable.Clear();
        PlaceHolder.Clear();
    }

    public virtual void PickedTile(Vector2Int[] TargetPos, Vector2Int standingPos_optional) {
        if (TargetPos.Length < 1)
            return;

        if (!Tiles.Contains(TargetPos[0]))
            return;

        List<UnitManager> Targets = new List<UnitManager>();
        currentCooldown = Cooldown;

        foreach (var pos in TargetPos) {
            if (AbilityApplicable.ContainsKey(pos))
                Targets.Add(AbilityApplicable[pos]);
        }

        if (Runner.gridPos == standingPos_optional) {
            Runner.ActionQueue.Enqueue(new UnitAbility(Runner, this, Runner.gameObject, TargetPos, Targets.ToArray(), Runner.turnManager.Tiles[TargetPos[0]]));
            Runner.ResetTiles();
        }
        else {
            Runner.ActionQueue.Enqueue(new UnitMoveToTile(Runner, Runner.pathfinding.FindPathToTile(Runner.gridPos, standingPos_optional, Runner.TileParents)));
            Runner.ActionQueue.Enqueue(new UnitAbility(Runner, this, Runner.gameObject, TargetPos, Targets.ToArray(), Runner.turnManager.Tiles[TargetPos[0]]));
            Runner.ResetTiles();
        }
            
        OnExit();
    }

    public abstract void WhatItDoes(Vector2Int[] pos, UnitManager[] targets);

    public virtual string ToolTipText(List<HealthComponent> targetHealthComponents) {
        return "Yet To Implement";
    }
}