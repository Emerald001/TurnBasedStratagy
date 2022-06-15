using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour, IState
{
    public bool IsDone { get; set; }

    public Transform CamPos;
    public MoveComponent moveComponent;

    public void StartGame() {
        IsDone = true;

    }

    public void CloseGame() {
        Application.Quit();
    }

    public void OnUpdate() {
        
    }

    public void OnEnter() {
        Camera.main.transform.SetPositionAndRotation(CamPos.position, CamPos.rotation);
        GameManager.instance.PauseMenu.SetActive(false);
        GameManager.instance.audiomanager.PlayLoopedAudio("Music", true);

        moveComponent.Move = true;
    }

    public void OnExit() {
        moveComponent.Move = false;
        gameObject.SetActive(false);
    }
}