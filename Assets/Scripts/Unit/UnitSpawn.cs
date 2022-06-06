using System.Collections.Generic;
using UnityEngine;

namespace UnitComponents {
    public static class UnitSpawn {
        public static void SpawnUnits(TurnManager turnManager, GameObject prefab, UnitBase values, Vector2Int gridPos, List<UnitManager> listToAddTo, List<UnitManager> enemyList, Transform unitParent) {
            var worldPos = UnitStaticFunctions.CalcWorldPos(gridPos);

            //create the Unit
            var Unit = GameObject.Instantiate(prefab, worldPos, Quaternion.identity);
            Unit.name = values.name;
            Unit.transform.parent = unitParent;
            HealthComponent healthComponent = new HealthComponent();

            var model = GameObject.Instantiate(values.Model, worldPos, Quaternion.identity);
            var visuals = Unit.transform.GetChild(0);
            model.transform.parent = visuals;

            if (gridPos.x > turnManager.gridWidth / 2)
                visuals.LookAt(visuals.transform.position + new Vector3(-1, 0, 0));
            else
                visuals.LookAt(visuals.transform.position + new Vector3(1, 0, 0));

            var UnitScript = Unit.GetComponent<UnitManager>();
            UnitScript.HealthComponent = healthComponent;
            healthComponent.owner = UnitScript;

            //set Scripts
            if (values.isRanged)
                UnitScript.defineAttackableTiles = new UnitDefineAttackableTilesRanged();
            else
                UnitScript.defineAttackableTiles = new UnitDefineAttackableTilesMelee();

            UnitScript.pathfinding = new UnitPathfinding();
            UnitScript.defineAccessableTiles = new UnitDefineAccessableTiles();

            foreach(var ability in values.abilities)
                UnitScript.abilities.Add(ScriptableObject.Instantiate(ability));

            //Give Values
            UnitScript.turnManager = turnManager;
            UnitScript.gridPos = gridPos;
            UnitScript.EnemyList = enemyList;
            UnitScript.OwnList = listToAddTo;

            //Get values from Scriptable Object
            var unitValues = UnitScript.values = new UnitValues();
            unitValues.owner = UnitScript;
            unitValues.baseDamageValue = values.baseDamageValue;
            unitValues.baseInitiativeValue = values.baseInitiativeValue;
            unitValues.baseSpeedValue = values.baseSpeedValue;
            unitValues.baseRangeValue = values.baseRangeValue;
            unitValues.baseHealthValue = values.baseHealthValue;
            unitValues.baseDefenceValue = values.baseDefenceValue;

            //run setvalues
            unitValues.SetValues();

            //give and set health
            healthComponent.OnEnter();

            //add to lists for better accessability
            listToAddTo.Add(UnitScript);
            turnManager.AllUnitsInPlay.Add(UnitScript);
            turnManager.LivingUnitsInPlay.Add(UnitScript);
        }
    }
}