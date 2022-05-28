using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenWorldState : IState {

    public GameObject OpenWorld;
    public bool IsDone { get; set; }

    private float timeToWait = 3f;

    public OpenWorldState(GameObject openworld) {
        OpenWorld = openworld;
    }

    public void OnEnter() {
        OpenWorld.SetActive(true);
    }

    public void OnExit() {
        OpenWorld.SetActive(false);
    }

    public void OnUpdate() {
        if (timer())
            IsDone = true;
    }

    bool timer() {
        return (timeToWait -= Time.deltaTime) < 0;
    }
}