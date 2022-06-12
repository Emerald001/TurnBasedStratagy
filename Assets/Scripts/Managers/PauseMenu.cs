using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject Panel;
    public MoveComponent moveComponent;

    private bool Paused = false; 

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            OpenAndClosePanel();
        }
    }

    public void OpenAndClosePanel() {
        if (Paused) {
            Panel.SetActive(false);
            Paused = false;
            moveComponent.Move = false;
            GameManager.instance.Player.GetComponent<Movement>().interactable = true;
        }
        else {
            Panel.SetActive(true);
            Paused = true;
            moveComponent.Move = true;
            GameManager.instance.Player.GetComponent<Movement>().interactable = false;
        }
    }

    public void MainMenu() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}