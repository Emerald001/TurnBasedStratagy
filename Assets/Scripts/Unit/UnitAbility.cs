using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitComponents { 
    public class UnitAbility : UnitAction
    {
        public UnitAbility(UnitManager Runner, AbilityBase abilityBase, GameObject Unit, Vector2Int[] positions, UnitManager[] targets, GameObject altTarget) {
            this.Runner = Runner;
            this.abilityBase = abilityBase;
            this.Unit = Unit;
            this.positions = positions;
            this.targets = targets;
            this.altTarget = altTarget;

            if (Runner.UnitAnimator.AnimationTimes.ContainsKey(abilityBase.AnimationTrigger))
                abilityTimer = Runner.UnitAnimator.AnimationTimes[abilityBase.AnimationTrigger] - .5f;

            EndsTurn = abilityBase.EndsTurn;
        }

        private AbilityBase abilityBase;
        private GameObject Unit;
        private UnitManager Runner;
        private GameObject altTarget;
        private Vector2Int[] positions;
        private UnitManager[] targets;

        private float abilityTimer = 0;
        private bool DoneAnimation = false;
        private bool DoneAbility = false;

        public override void OnUpdate() {
            if (!MustTurn())
                return;

            if (!DoneAbility) {
                RunAbility();
                return;
            }

            IsDone = true;
        }

        public void RunAbility() {
            if(targets.Length > 0 && targets.Length < 2) {
                if(targets[0].gameObject != Unit)
                    Unit.transform.GetChild(0).LookAt(targets[0].transform);
            }

            if (!DoneAnimation) {
                Runner.UnitAnimator.AbilityAnim(abilityBase.AnimationTrigger);
                DoneAnimation = true;
            }

            if(Timer(ref abilityTimer) < 0) {
                abilityBase.WhatItDoes(positions, targets);
                DoneAbility = true;
            }
        }

        public bool MustTurn() {
            if (targets.Length > 0 && targets.Length < 2) {
                if (targets[0].gameObject != Unit) 
                    if (Unit.transform.GetChild(0).rotation != Quaternion.LookRotation(targets[0].transform.position - Unit.transform.position)) {
                        Unit.transform.GetChild(0).rotation = Quaternion.RotateTowards(Unit.transform.GetChild(0).rotation, Quaternion.LookRotation(targets[0].transform.position - Unit.transform.position), 360f * Time.deltaTime);
                        return false;
                    }
            }
            else if(targets.Length > 1 || targets.Length == 0) { 
                if(altTarget != null) 
                    if (Unit.transform.GetChild(0).rotation != Quaternion.LookRotation(altTarget.transform.position - Unit.transform.position)) {
                        Unit.transform.GetChild(0).rotation = Quaternion.RotateTowards(Unit.transform.GetChild(0).rotation, Quaternion.LookRotation(altTarget.transform.position - Unit.transform.position), 360f * Time.deltaTime);
                        return false;
                    }
            }

            return true;
        }

        public float Timer(ref float timer) {
            return timer -= Time.deltaTime;
        }
    }
}