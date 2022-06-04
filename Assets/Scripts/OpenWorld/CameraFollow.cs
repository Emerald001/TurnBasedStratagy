using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField] private float lookAroundSpeed;
    [SerializeField] private GameObject followObject;
    [SerializeField] private Vector3 offset;

    private Vector2 rotation;

    public void OnEnter() {
        transform.eulerAngles = new Vector3(0, 0, 0);
        Camera.main.transform.position = transform.position + offset;
        Camera.main.transform.eulerAngles = new Vector3(45, 0, 0);
    }

    void Update() { 
        transform.position = Vector3.Slerp(transform.position, followObject.transform.position, 5 * Time.deltaTime);

        if (Input.GetKey(KeyCode.Mouse1)) {
            rotation.y += Input.GetAxisRaw("Mouse X");
            rotation.x += -Input.GetAxisRaw("Mouse Y");
            rotation.x = Mathf.Clamp(rotation.x, -1, 4.5f);

            transform.eulerAngles = rotation * (lookAroundSpeed);
        }
    }
}