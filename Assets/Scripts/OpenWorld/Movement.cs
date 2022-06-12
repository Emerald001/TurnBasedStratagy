using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour {
    [SerializeField] private Animator animator;
    
    private NavMeshAgent agent;
    [HideInInspector] public bool interactable = false;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update() {
        if (!interactable)
            return;

        if (Input.GetKey(KeyCode.Mouse0)) {
            animator.SetBool("Walking", true);
            agent.destination = MouseValues.HoverPointPos;
        }

        if(transform.position == agent.destination) {
            animator.SetBool("Walking", false);
        }
    }
}