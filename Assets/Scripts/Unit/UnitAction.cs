using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitComponents {
    public abstract class UnitAction
    {
        public bool IsDone;
        public bool EndsTurn;
        public virtual void OnUpdate() { }
    }
}