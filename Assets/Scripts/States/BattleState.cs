using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : IState {

    public List<UnitBase> Enemies;
    private GameObject TurnManager;

    public BattleState (List<UnitBase> enemies, GameObject turnManager) {
        Enemies = enemies;
        TurnManager = turnManager;
    }

    public bool IsDone { get; set; }

    public void OnEnter() {

    }

    public void OnExit() {
        Debug.Log("Exit Battle state");
        IsDone = true;
    }

    public void OnUpdate() {
        Debug.Log("Update Battle state");
    }
}