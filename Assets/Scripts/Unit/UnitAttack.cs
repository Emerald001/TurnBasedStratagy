using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitComponents {
    public class UnitAttack : UnitAction
    {
        public float timer = 1f;

        public UnitAttack(GameObject Unit, GameObject Enemy, int Damage) {
            this.Unit = Unit;
            this.Enemy = Enemy;
            this.Damage = Damage;

            EndsTurn = true;
        }

        private GameObject Unit;
        private GameObject Enemy;
        private int Damage;

        public override void OnUpdate() {
            if (WaitTime() > 0)
                return;

            Attack();
        }

        public void Attack() {
            Unit.transform.GetChild(0).LookAt(Enemy.transform);
            Debug.Log("Attack!");
            Enemy.GetComponent<HealthComponent>().TakeDamage(Damage);
            IsDone = true;
        }

        public float WaitTime() {
            Unit.transform.GetChild(0).rotation = Quaternion.RotateTowards(Unit.transform.GetChild(0).rotation, Quaternion.LookRotation(Enemy.transform.position - Unit.transform.position), 360f * Time.deltaTime);
            return timer -= Time.deltaTime;
        }
    }
}