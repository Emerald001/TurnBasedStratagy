using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitComponents { 
    public class UnitMoveToTile : UnitAction {
        public UnitMoveToTile(UnitManager manager, List<Vector2Int> path) {
            Owner = manager;
            Path = path;
        }

        private UnitManager Owner;
        private List<Vector2Int> Path;

        public override void OnUpdate() {            
            MoveToTile(ref Owner.gridPos, ref Owner.values.speedValue);
        }

        public void MoveToTile(ref Vector2Int gridPos, ref int speedValue) {
            var Unit = Owner.Unit.transform;

            if (Unit.position != UnitStaticFunctions.CalcWorldPos(Path[0])) {
                Unit.GetChild(0).rotation = Quaternion.RotateTowards(Unit.GetChild(0).rotation, Quaternion.LookRotation(UnitStaticFunctions.CalcWorldPos(Path[0]) - Unit.position), 360f * Time.deltaTime);
                Unit.position = Vector3.MoveTowards(Unit.position, UnitStaticFunctions.CalcWorldPos(Path[0]), 4 * Time.deltaTime);
            }
            else {
                gridPos = Path[0];
                speedValue--;
                Path.RemoveAt(0);
            }

            if (Path.Count < 1) {
                if (speedValue < 1)
                    EndsTurn = true;
                IsDone = true;
            }
        }
    }
}