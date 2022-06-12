using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenWorldState : IState 
{
    public GameObject OpenWorld;
    public GameObject SmallTutorial;

    public bool IsDone { get; set; }

    public OpenWorldState(GameObject openworld, GameObject SmallTutorial) {
        OpenWorld = openworld;
        this.SmallTutorial = SmallTutorial;
    }

    public void OnEnter() {
        OpenWorld.SetActive(true);
        SmallTutorial.SetActive(true);
        GameManager.instance.PauseMenu.SetActive(true);
        
        var camFollowObject = OpenWorld.transform.GetChild(0);
        Camera.main.transform.parent = camFollowObject;
        camFollowObject.GetComponent<CameraFollow>().OnEnter();

        GameManager.instance.Player.GetComponent<Movement>().interactable = true;
        
        IsDone = true;
    }

    public void OnExit() {
        Camera.main.transform.parent = null;

        GameManager.instance.Player.GetComponent<Movement>().interactable = false;
        SmallTutorial.SetActive(false);
        OpenWorld.SetActive(false);
    }

    public void OnUpdate() {
    }
}