using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    protected int targetFPS = 144;

    //Remove singleton at some point
    public static GameManager instance;
    private void Awake() {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = targetFPS;
        instance = this;
    }

    public GameObject Player;
    public GameObject PauseMenu;

    [SerializeField] private GameObject TurnManager;
    [SerializeField] private GameObject OpenWorld;
    [SerializeField] private GameObject SmallTutorial;
    [SerializeField] private MainMenu menustate;

    private StateMachine stateMachine = new StateMachine();

    private OpenWorldState openWorld;

    void Start() {
        openWorld = new OpenWorldState(OpenWorld, SmallTutorial);
        stateMachine.OnEnter(menustate);
        stateMachine.AddState(openWorld);
    }
    
    void Update() {
        if (Application.targetFrameRate != targetFPS)
            Application.targetFrameRate = targetFPS;

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