using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitComponents {
    public class UnitAttack : UnitAction
    {
        public float timer = 1.5f;
        private bool DoneAttack = false;

        public UnitAttack(UnitManager Owner, GameObject Unit, UnitManager Enemy, int Damage) {
            this.Owner = Owner;
            this.Unit = Unit;
            this.Enemy = Enemy;
            this.Damage = Damage;

            EndsTurn = true;
        }

        private UnitManager Enemy;
        private UnitManager Owner;
        private GameObject Unit;
        private int Damage;

        public override void OnUpdate() {
            if (!MustWait()) 
                return;

            if(!DoneAttack)
                Attack();

            if (Timer() < 0)
                IsDone = true;
        }

        public void Attack() {
            Unit.transform.GetChild(0).LookAt(Enemy.transform);
            Owner.UnitAnimator.AttackAnim();
            Enemy.HealthComponent.TakeDamage(Damage);
            Owner.UnitAnimator.HitEnemy(Enemy);
            DoneAttack = true;
        }

        public bool MustWait() {
            if(Unit.transform.GetChild(0).rotation != Quaternion.LookRotation(Enemy.transform.position - Unit.transform.position)) {
                Unit.transform.GetChild(0).rotation = Quaternion.RotateTowards(Unit.transform.GetChild(0).rotation, Quaternion.LookRotation(Enemy.transform.position - Unit.transform.position), 360f * Time.deltaTime);
                return false;
            }
            return true;
        }
        public float Timer() {
            return timer -= Time.deltaTime;
        }
    }
}