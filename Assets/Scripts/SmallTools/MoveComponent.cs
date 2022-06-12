using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveComponent : MonoBehaviour
{
    public Vector2 newPos;

    private Vector2 StartingPos;
    private RectTransform objectToMove;

    [HideInInspector] public bool Move = false;

    void Start() {
        objectToMove = GetComponent<RectTransform>();
        StartingPos = objectToMove.localPosition;
    }

    void Update() {
        if (Move)
            objectToMove.localPosition = Vector3.Lerp(objectToMove.localPosition, newPos, 10 * Time.deltaTime);
        else
            objectToMove.localPosition = Vector3.Lerp(objectToMove.localPosition, StartingPos, 10 * Time.deltaTime);
    }
}