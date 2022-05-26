using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuState : IState {
    public bool IsDone { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void OnEnter() {
        Debug.Log("Enter Menu state");
    }

    public void OnExit() {
        Debug.Log("Exit Menu state");
    }

    public void OnUpdate() {
        Debug.Log("Update Menu state");
    }
}