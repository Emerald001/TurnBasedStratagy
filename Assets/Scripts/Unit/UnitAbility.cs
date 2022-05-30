using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitComponents { 
    public class UnitAbility : UnitAction
    {
        public UnitAbility(AbilityBase abilityBase, List<Vector2Int> positions, Dictionary<Vector2Int, GameObject> enemyPositions) {
            ValueBase = abilityBase;
            pickedTiles = positions;
            EnemyPositions = enemyPositions;
            EndsTurn = abilityBase.EndsTurn;
        }

        private Dictionary<Vector2Int, GameObject> EnemyPositions;
        private List<Vector2Int> pickedTiles;
        private AbilityBase ValueBase;

        public override void OnUpdate() { 
            
        }
    }
}