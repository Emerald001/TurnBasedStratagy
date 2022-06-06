using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitComponents { 
    public class UnitAbility : UnitAction
    {
        public UnitAbility(AbilityBase abilityBase, GameObject Unit, Vector2Int[] positions, UnitManager[] targets) {
            this.abilityBase = abilityBase;
            this.Unit = Unit;
            this.positions = positions;
            this.targets = targets;

            EndsTurn = abilityBase.EndsTurn;
        }

        private AbilityBase abilityBase;
        private GameObject Unit;
        private Vector2Int[] positions;
        private UnitManager[] targets;
        private float timer = 1f;

        public override void OnUpdate() {
            if (WaitTime() > 0)
                return;

            RunAbility();
        }

        public void RunAbility() {
            if(targets.Length > 0 && targets.Length < 2) {
                UnitManager target = targets[0];
                if(target.gameObject != Unit)
                    Unit.transform.GetChild(0).LookAt(target.transform);
            }

            abilityBase.WhatItDoes(positions, targets);
            IsDone = true;
        }

        public float WaitTime() {
            if (targets.Length > 0 && targets.Length < 2)
                if (targets[0].gameObject != Unit)
                    Unit.transform.GetChild(0).rotation = Quaternion.RotateTowards(Unit.transform.GetChild(0).rotation, Quaternion.LookRotation(targets[0].transform.position - Unit.transform.position), 360f * Time.deltaTime);

            return timer -= Time.deltaTime;
        }
    }
}