using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor.Animations;

public class EnemyNPC : MonoBehaviour
{
    [SerializeField] private float viewDistance = 10;
    [SerializeField] private float battleStartDistance = 2;
    
    public int teamSize = 5;
    private List<UnitBase> Team = new List<UnitBase>();

    private GameObject Player;
    private GameObject Visuals;
    private Animator animator;
    private NavMeshAgent agent;
    private Vector3 StartingPosition;

    void Start() {
        StartingPosition = transform.position;

        agent = GetComponent<NavMeshAgent>();
        Player = GameManager.instance.Player;

        var allEnemies = Resources.LoadAll<UnitBase>("Units/EnemyUnits/");
        for (int i = 0; i < teamSize; i++) {
            Team.Add(allEnemies[Random.Range(0, allEnemies.Length)]);
        }

        var teamleader = Team[0];
        Visuals = transform.GetChild(0).GetChild(0).gameObject;
        animator = Visuals.GetComponent<Animator>();
        animator.runtimeAnimatorController = teamleader.Animator;

        var WeaponHolder = Visuals.GetComponentInChildren<WeaponHolderReference>();
        if (teamleader.Weapon != null)
            GameObject.Instantiate(teamleader.Weapon, WeaponHolder.transform);
    }

    void Update() {
        if (Vector3.Distance(transform.position, Player.transform.position) < viewDistance) {
            agent.SetDestination(Player.transform.position);
            animator.SetBool("Walking", true);
        }
        else {
            agent.SetDestination(StartingPosition);
        }

        if (Vector3.Distance(transform.position, Player.transform.position) < battleStartDistance) {
            GameManager.instance.AddBattleState(Team);
            Destroy(this.gameObject);
        }

        if(transform.position == agent.destination) {
            animator.SetBool("Walking", false);
        }
    }
}