using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    public bool IsDone { get; set; }

    public void OnUpdate();
    public void OnEnter();
    public void OnExit();
}