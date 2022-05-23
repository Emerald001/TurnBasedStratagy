using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour {
    public Material BaseColor;
    public Material GivenColor;

    public void SetColor(Material color) {
        this.GetComponentInChildren<Renderer>().material = color;
    }

    public void ResetColor() {
        if(GivenColor == null)
            this.GetComponentInChildren<Renderer>().material = BaseColor;
        else
            this.GetComponentInChildren<Renderer>().material = GivenColor;
    }
}