using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNPC : MonoBehaviour
{
    [SerializeField] private float viewDistance = 10;
    [SerializeField] private float battleStartDistance = 2;
    [SerializeField] private float Difficulty = 2;
    [SerializeField] private List<Transform> Waypoints;
    private int CurrentWaypoint = 0;

    public int teamSize = 5;
    private List<UnitBase> Team = new List<UnitBase>();

    private GameObject Player;
    private GameObject Visuals;
    private Animator animator;
    private NavMeshAgent agent;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(Waypoints[CurrentWaypoint].position);
        Player = GameManager.instance.Player;

        var allEnemies = Resources.LoadAll<UnitBase>("Units/EnemyUnits/Difficulty" + Difficulty);
        for (int i = 0; i < teamSize; i++) {
            Team.Add(allEnemies[Random.Range(0, allEnemies.Length)]);
        }

        var teamleader = Team[0];
        Visuals = Instantiate(teamleader.Model, transform.GetChild(0));

        var bands = Visuals.GetComponentsInChildren<ColorHolder>();
        foreach (var item in bands) {
            GameObject.Instantiate(teamleader.Band, item.gameObject.transform);
        }

        animator = Visuals.GetComponent<Animator>();

        animator.SetBool("Walking", true);

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
            WalkAround();
        }

        if (Vector3.Distance(transform.position, Player.transform.position) < battleStartDistance) {
            GameManager.instance.AddBattleState(Team);
            Destroy(this.gameObject);
        }

        if(transform.position == agent.destination) {
            animator.SetBool("Walking", false);
        }
    }

    void WalkAround() {
        if(Vector3.Distance(agent.destination, transform.position) > 1) {
            animator.SetBool("Walking", true);
            return;
        }

        if(CurrentWaypoint < Waypoints.Count - 1) {
            animator.SetBool("Walking", true);
            CurrentWaypoint++;
            agent.SetDestination(Waypoints[CurrentWaypoint].position);
        }
        else {
            CurrentWaypoint = 0;
            agent.SetDestination(Waypoints[CurrentWaypoint].position);
        }
    }
}