using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : IState {
    public List<UnitBase> Enemies;
    public List<UnitBase> PlayerUnits;

    private GameObject TurnManager;

    private TurnManager currentManager;

    public BattleState (List<UnitBase> enemies, List<UnitBase> playerUnits, GameObject turnManager) {
        Enemies = enemies;
        PlayerUnits = playerUnits;
        TurnManager = turnManager;
    }

    public bool IsDone { get; set; }

    public void OnEnter() {
        currentManager = GameObject.Instantiate(TurnManager).GetComponent<TurnManager>();
        currentManager.PlayerUnitsToSpawn = PlayerUnits;
        currentManager.EnemiesToSpawn = Enemies;
    }

    public void OnExit() {
        GameObject.Destroy(TurnManager);

        Debug.Log("Exit Battle state");
    }

    public void OnUpdate() {
        if (TurnManager.GetComponent<TurnManager>().isDone)
            IsDone = true;
    }
}