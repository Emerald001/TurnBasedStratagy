using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitComponents { 
    public class UnitMoveToTile : UnitAction {
        public UnitMoveToTile(UnitManager manager) {
            Owner = manager;
        }

        private UnitManager Owner;

        public override void OnUpdate() {
            MoveToTile(Owner.currentPath, ref Owner.gridPos, ref Owner.speedValue);
        }

        public virtual void MoveToTile(List<Vector2Int> path, ref Vector2Int gridPos, ref int speedValue) {
            var Unit = Owner.Unit.transform;

            if (Unit.position != UnitStaticFunctions.CalcWorldPos(path[0])) {
                Unit.rotation = Quaternion.RotateTowards(Unit.rotation, Quaternion.LookRotation(UnitStaticFunctions.CalcWorldPos(path[0]) - Unit.position), 360f * Time.deltaTime);
                Unit.position = Vector3.MoveTowards(Unit.position, UnitStaticFunctions.CalcWorldPos(path[0]), 4 * Time.deltaTime);
            }
            else {
                gridPos = path[0];
                speedValue--;
                path.RemoveAt(0);
            }

            if (path.Count < 1) {
                IsDone = true;
            }
        }
    }
}