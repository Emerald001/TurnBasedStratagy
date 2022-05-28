using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject followObject;
    public Vector3 offset;

    void Update() { 
        transform.position = Vector3.Slerp(transform.position, followObject.transform.position + offset, 5 * Time.deltaTime);
        Camera.main.transform.position = transform.position;
    }
}
