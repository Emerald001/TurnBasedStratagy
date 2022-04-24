using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitComponents { 
    public class UnitPathfinding {
        public List<Vector2Int> FindPathToTile(Vector2Int EndPos, Vector2Int StartPos, Dictionary<Vector2Int, Vector2Int> tileParents) {
            var path = new List<Vector2Int>();
            var currentPos = StartPos;

            while (currentPos != EndPos) {
                path.Add(currentPos);
                currentPos = tileParents[currentPos];
            }

            //path.Add(currentPos);
            path.Reverse();

            return path;
        }
    }
}