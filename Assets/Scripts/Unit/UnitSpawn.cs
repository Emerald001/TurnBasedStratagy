using System.Collections.Generic;
using UnityEngine;

namespace UnitComponents {
    public static class UnitSpawn {
        public static void SpawnUnits(TurnManager turnManager, GameObject prefab, UnitBase values, Vector2Int gridPos, List<UnitManager> listToAddTo, List<UnitManager> enemyList, Transform unitParent) {
            Vector3 worldPos = UnitStaticFunctions.CalcWorldPos(gridPos);

            // Create the Unit.
            GameObject Unit = Object.Instantiate(prefab, worldPos, Quaternion.identity);
            Unit.name = values.name;
            Unit.transform.parent = unitParent;

            // Create the Model.
            GameObject model = Object.Instantiate(values.Model, worldPos, Quaternion.identity);
            Transform visuals = Unit.transform.GetChild(0);
            model.transform.parent = visuals;

            // Set Colors.
            ColorHolder[] bands = model.GetComponentsInChildren<ColorHolder>();
            foreach(ColorHolder item in bands)
                Object.Instantiate(values.Band, item.gameObject.transform);

            if (gridPos.x > turnManager.BattleSettings.gridWidth / 2)
                visuals.LookAt(visuals.transform.position + new Vector3(-1, 0, 0));
            else
                visuals.LookAt(visuals.transform.position + new Vector3(1, 0, 0));

            UnitManager UnitScript = Unit.GetComponent<UnitManager>();
            UnitScript.Icon = values.Icon;

            // Set Animator.
            UnitAnimationManager AnimationScript = Unit.AddComponent<UnitAnimationManager>();
            UnitScript.UnitAnimator = AnimationScript;
            AnimationScript.Owner = UnitScript;
            AnimationScript.Init();
            if(values.Weapon != null)
                Object.Instantiate(values.Weapon, AnimationScript.WeaponHolder.transform);

            // Set Scripts.
            if (values.isRanged)
                UnitScript.defineAttackableTiles = new UnitDefineAttackableTilesRanged();
            else
                UnitScript.defineAttackableTiles = new UnitDefineAttackableTilesMelee();

            UnitScript.pathfinding = new UnitPathfinding();
            UnitScript.defineAccessableTiles = new UnitDefineAccessableTiles();

            foreach(AbilityBase ability in values.abilities)
                UnitScript.abilities.Add(Object.Instantiate(ability));

            // Set AudioManager.
            UnitAudioManager AudioManager = Unit.AddComponent<UnitAudioManager>();
            UnitScript.UnitAudio = AudioManager;

            if(values.sounds != null)
                foreach (Sound s in values.sounds)
                    AudioManager.soundslist.Add(s);

            foreach (AbilityBase abilitySound in UnitScript.abilities)
                AudioManager.soundslist.Add(abilitySound.Sound);
            AudioManager.Init();

            // Give Values.
            UnitScript.turnManager = turnManager;
            UnitScript.gridPos = gridPos;
            UnitScript.EnemyList = enemyList;
            UnitScript.OwnList = listToAddTo;

            // Get values from Scriptable Object.
            var unitValues = UnitScript.values = new UnitValues();
            unitValues.owner = UnitScript;
            unitValues.baseDamageValue = values.baseDamageValue;
            unitValues.baseInitiativeValue = values.baseInitiativeValue;
            unitValues.baseSpeedValue = values.baseSpeedValue;
            unitValues.baseRangeValue = values.baseRangeValue;
            unitValues.baseHealthValue = values.baseHealthValue;
            unitValues.baseDefenceValue = values.baseDefenceValue;

            // Run setvalues.
            unitValues.SetValues();

            // Give and set health.
            HealthComponent healthComponent = new HealthComponent();
            UnitScript.HealthComponent = healthComponent;
            healthComponent.Owner = UnitScript;
            healthComponent.OnEnter();

            // Add to lists for better accessability.
            listToAddTo.Add(UnitScript);
            turnManager.AllUnitsInPlay.Add(UnitScript);
            turnManager.LivingUnitsInPlay.Add(UnitScript);
            turnManager.UnitPositions.Add(UnitScript, gridPos);
        }
    }
}