using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitComponents { 
    public abstract class UnitDefineAttackableTiles : MonoBehaviour
    {
        public virtual List<Vector2Int> FindAttackableTiles(List<Unit> AttackList, Dictionary<Vector2Int, GameObject> grid) {
            return new List<Vector2Int>();
        }
        public virtual Vector2Int GetClosestTile(Vector2Int tile, Vector3 worldpoint, List<Vector2Int> accessableTiles) {
            return new Vector2Int();
        }
    }
}