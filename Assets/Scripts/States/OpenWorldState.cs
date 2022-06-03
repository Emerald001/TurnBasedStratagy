using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenWorldState : IState {

    public GameObject OpenWorld;
    public bool IsDone { get; set; }

    public OpenWorldState(GameObject openworld) {
        OpenWorld = openworld;
    }

    public void OnEnter() {
        OpenWorld.SetActive(true);
        
        var camFollowObject = OpenWorld.transform.GetChild(0);
        Camera.main.transform.parent = camFollowObject;
        camFollowObject.GetComponent<CameraFollow>().OnEnter();
    }

    public void OnExit() {
        Camera.main.transform.parent = null;

        OpenWorld.SetActive(false);
    }

    public void OnUpdate() {
        IsDone = true;
    }
}