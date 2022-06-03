using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNPC : MonoBehaviour
{
    [SerializeField] private float viewDistance = 10;
    [SerializeField] private float battleStartDistance = 2;
    
    public int teamSize = 5;
    private List<UnitBase> Team = new List<UnitBase>();

    private GameObject Player;
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
    }

    void Update() {
        if (Vector3.Distance(transform.position, Player.transform.position) < viewDistance)
            agent.SetDestination(Player.transform.position);
        else
            agent.SetDestination(StartingPosition);

        if (Vector3.Distance(transform.position, Player.transform.position) < battleStartDistance) {
            GameManager.instance.AddBattleState(Team);
            Destroy(this.gameObject);
        }
    }
}