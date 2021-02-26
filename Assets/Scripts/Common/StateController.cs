using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public State currentState;
    [Tooltip("This is the default state called RemainInState. If the decision object decides it does not need to change states, it will return this state")]
    public State RemainInState;

    private bool aiActive = true;
    // Start is called before the first frame update
    private void Awake()
    {
        print("AI Starting");
    }

    // Update is called once per frame
    void Update()
    {
        if (!aiActive) return;
        
        currentState.UpdateState(this);
    }

    public void TransitionToState(State nextState)
    {
        if (nextState != RemainInState)
        {
            currentState = nextState;
        }
    }
}
