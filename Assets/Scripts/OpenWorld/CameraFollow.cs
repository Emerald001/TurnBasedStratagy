using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField] private float lookAroundSpeed;
    [SerializeField] private GameObject followObject;
    [SerializeField] private Vector3 offset;
    
    [SerializeField] private GameObject SmallTutorial;

    private Vector2 rotation;

    public void OnEnter() {
        transform.eulerAngles = new Vector3(0, 0, 0);
        Camera.main.transform.position = transform.position + offset;
        Camera.main.transform.eulerAngles = new Vector3(45, 0, 0);
    }

    void Update() { 
        transform.position = Vector3.Slerp(transform.position, followObject.transform.position, 5 * Time.deltaTime);
        transform.eulerAngles = rotation * (lookAroundSpeed);

        if (Input.GetKey(KeyCode.Mouse1)) {
            SmallTutorial.SetActive(false);

            rotation.y += Input.GetAxisRaw("Mouse X");
            rotation.x += -Input.GetAxisRaw("Mouse Y");
            rotation.x = Mathf.Clamp(rotation.x, -1, 4.5f);
        }
    }
}