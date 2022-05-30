using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUIManager : MonoBehaviour
{
    public Text OrderNumber;
    public Slider HealthBar;

    private void Awake() {
        
    }

    public void UpdateHealth(float newHealth) {
        HealthBar.value = newHealth;
    }
}