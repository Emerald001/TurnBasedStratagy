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

        GameManager.instance.audiomanager.PlayLoopedAudio("BattleMusic", true);
    }

    public void OnExit() {
        GameManager.instance.audiomanager.PlayLoopedAudio("BattleMusic", false);
        GameObject.Destroy(currentManager.gameObject);
    }

    public void OnUpdate() {
        if (currentManager.isDone)
            IsDone = true;
    }
}