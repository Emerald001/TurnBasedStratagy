using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private MoveComponent moveComponent;
    private GameObject Panel;

    private bool Paused = false;

    void Start() {
        moveComponent = GetComponentInChildren<MoveComponent>();
        Panel = transform.GetChild(0).gameObject;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            if (Paused) {
                Panel.SetActive(false);
                Paused = false;
                moveComponent.Move = false;
            }
            else {
                Panel.SetActive(true);
                Paused = true;
                moveComponent.Move = true;
            }
        }
    }
}
