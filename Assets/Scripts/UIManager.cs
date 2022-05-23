using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public IEnumerator MoveUI() {
        yield return new WaitForEndOfFrame();
    }
}