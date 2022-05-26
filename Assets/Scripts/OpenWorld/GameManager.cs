using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject TurnManager;

    private StateMachine stateMachine = new StateMachine();

    private MenuState menustate = new MenuState();
    private OpenWorldState openWorld = new OpenWorldState();

    void Start() {
        stateMachine.OnEnter(openWorld);

        var list = new List<UnitBase>();
        var SO = Resources.LoadAll<UnitBase>("Units/EnemyUnits/");
        foreach(UnitBase unit in SO) {
            list.Add(unit);
        }
        AddBattleState(list);
    }

    void Update() {
        stateMachine.OnUpdate();
    }

    public void AddBattleState(List<UnitBase> EnemyList) {
        stateMachine.AddState(new BattleState(EnemyList, TurnManager));
    }
}