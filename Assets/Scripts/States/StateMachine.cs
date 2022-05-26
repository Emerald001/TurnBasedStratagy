using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private IState currentState;
    private readonly Queue<IState> stateQueue = new Queue<IState>();

    public void OnEnter(IState state) {
        currentState = state;
    }

    public void OnUpdate() {
        if (currentState == null)
            return;
        
        currentState.OnUpdate();

        if (currentState.IsDone) {
            if(stateQueue.Count > 0) {
                SwitchState(stateQueue.Dequeue());
            }
        }
    }

    private void SwitchState(IState state) {
        if (currentState != null)
            currentState.OnExit();

        currentState = state;
        currentState.OnEnter();
    }

    public void AddState(IState state) {
        stateQueue.Enqueue(state);
    }
}