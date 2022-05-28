using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Remove singleton at sonme point
    public static GameManager instance;
    private void Awake() {
        instance = this;
    }

    public GameObject Player;

    [SerializeField] private GameObject TurnManager;
    [SerializeField] private GameObject OpenWorld;

    private StateMachine stateMachine = new StateMachine();

    private MenuState menustate = new MenuState();
    private OpenWorldState openWorld;

    void Start() {
        openWorld = new OpenWorldState(OpenWorld);
        stateMachine.OnEnter(openWorld);
    }
    
    void Update() {
        stateMachine.OnUpdate();
    }

    public void AddBattleState(List<UnitBase> EnemyList) {
        var list = new List<UnitBase>();
        var SO = Resources.LoadAll<UnitBase>("Units/PlayerUnits/");
        foreach (UnitBase unit in SO) {
            list.Add(unit);
        }

        stateMachine.AddState(new BattleState(EnemyList, list, TurnManager));
        stateMachine.AddState(openWorld);
    }
}