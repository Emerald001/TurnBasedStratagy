using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitComponents {
    public class UnitAttack : UnitAction
    {
        public UnitAttack(GameObject Unit, GameObject Enemy, int Damage) {
            this.Unit = Unit;
            this.Enemy = Enemy;
            this.Damage = Damage;

            EndsTurn = true;
        }

        private GameObject Unit;
        private GameObject Enemy;
        private int Damage;

        public void Attack() {
            Unit.transform.GetChild(0).LookAt(Enemy.transform);
            Enemy.GetComponent<HealthComponent>().TakeDamage(Damage);
            IsDone = true;
        }
    }
}