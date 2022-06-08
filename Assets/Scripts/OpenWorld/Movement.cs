using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour {
    [SerializeField] private Animator animator;
    
    private NavMeshAgent agent;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update() {
        if (Input.GetKey(KeyCode.Mouse0)) {
            animator.SetBool("Walking", true);
            agent.destination = MouseValues.HoverPointPos;
        }

        if(transform.position == agent.destination) {
            animator.SetBool("Walking", false);
        }
    }
}