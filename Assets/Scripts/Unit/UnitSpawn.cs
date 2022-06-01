using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitComponents {
    public class UnitSpawn {
        public void SpawnUnits(TurnManager turnManager, GameObject prefab, UnitBase values, Vector2Int gridPos, List<UnitManager> listToAddTo, List<UnitManager> enemyList, Transform unitParent) {
            var worldPos = UnitStaticFunctions.CalcWorldPos(gridPos);

            //create the Unit
            var Unit = GameObject.Instantiate(prefab, worldPos, Quaternion.identity);
            Unit.name = values.name;
            Unit.transform.parent = unitParent;
            Unit.AddComponent<HealthComponent>();

            var UnitScript = Unit.GetComponent<UnitManager>();

            //set Scripts
            if (values.isRanged)
                UnitScript.defineAttackableTiles = new UnitDefineAttackableTilesRanged();
            else
                UnitScript.defineAttackableTiles = new UnitDefineAttackableTilesMelee();

            UnitScript.pathfinding = new UnitPathfinding();
            UnitScript.defineAccessableTiles = new UnitDefineAccessableTiles();
            UnitScript.defineAccessableTiles.Owner = UnitScript;
            UnitScript.defineAttackableTiles.Owner = UnitScript;
            UnitScript.abilities = values.abilities;

            //Give Values
            UnitScript.turnManager = turnManager;
            UnitScript.gridPos = gridPos;
            UnitScript.EnemyList = enemyList;

            //Get values from Scriptable Object
            var unitValues = UnitScript.values;
            unitValues.baseDamageValue = values.baseDamageValue;
            unitValues.baseInitiativeValue = values.baseInitiativeValue;
            unitValues.baseSpeedValue = values.baseSpeedValue;
            unitValues.baseRangeValue = values.baseRangeValue;

            //run setvalues
            unitValues.SetValues();

            //give and set health
            var HealthScript = UnitScript.GetComponent<HealthComponent>();
            HealthScript.baseHealthValue = values.baseHealthValue;
            HealthScript.baseDefenceValue = values.baseDefenceValue;

            HealthScript.Defence = HealthScript.baseDefenceValue;
            HealthScript.Health = HealthScript.baseHealthValue;

            //add to lists for better accessability
            listToAddTo.Add(UnitScript);
            turnManager.AllUnitsInPlay.Add(UnitScript);
            turnManager.LivingUnitsInPlay.Add(UnitScript);
        }
    }
}