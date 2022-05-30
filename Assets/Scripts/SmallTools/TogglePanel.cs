using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogglePanel : MonoBehaviour
{
    public void TurnOnPanel (GameObject panel) => panel.SetActive(true);

    public void TurnOffPanel (GameObject panel) => panel.SetActive(false);
}