using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFollow : MonoBehaviour
{
    public GameObject ObjectToFollow;

    public float maxDistance = 5;
    public float followingSpeed = 10;

    private void Update() {
        var distanceToObject = Vector3.Distance(transform.position, ObjectToFollow.transform.position);
        var currentSpeed = followingSpeed + distanceToObject;
        

        transform.LookAt(ObjectToFollow.transform);

        if(distanceToObject > maxDistance) {
            transform.position = Vector3.MoveTowards(transform.position, ObjectToFollow.transform.position, currentSpeed * Time.deltaTime);
        }

        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }
}