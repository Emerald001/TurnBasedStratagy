using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{
    public GameObject EndScreen;

    private void OnTriggerEnter(Collider other) {
        Debug.Log("AAAAAAAAAAAAA");

        if (other.CompareTag("Player")) {
            GameManager.instance.PauseMenu.GetComponent<PauseMenu>().OpenAndClosePanel();
            EndScreen.SetActive(true);
        }
    }
}
