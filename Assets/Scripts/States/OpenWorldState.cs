using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenWorldState : IState {
    public bool IsDone { get; set; }

    private float timeToWait = 3f;

    public void OnEnter() {
        Debug.Log("Enter Open World state");
    }

    public void OnExit() {
        Debug.Log("Exit Open World state");
    }

    public void OnUpdate() {
        Debug.Log("Update Open World state");

        if (timer())
            IsDone = true;
    }

    bool timer() {
        return (timeToWait -= Time.deltaTime) < 0;
    }
}