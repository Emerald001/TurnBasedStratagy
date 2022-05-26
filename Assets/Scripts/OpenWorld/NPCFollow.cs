using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCFollow : MonoBehaviour
{
    public GameObject ObjectToFollow;
    public float distanceToFollowAt;

    private NavMeshAgent agent;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        if(Vector3.Distance(transform.position, ObjectToFollow.transform.position) > distanceToFollowAt) {
            agent.SetDestination(ObjectToFollow.transform.position);
        }
        else {
            agent.SetDestination(transform.position);
        }
    }
}