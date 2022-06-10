using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour, IState
{
    public bool IsDone { get; set; }

    public void StartGame() {
        IsDone = true;
    }

    public void CloseGame() {
        Application.Quit();
    }

    public void OnUpdate() {
        
    }

    public void OnEnter() {
        GameManager.instance.PauseMenu.SetActive(false);
    }

    public void OnExit() {
        gameObject.SetActive(false);
    }
}