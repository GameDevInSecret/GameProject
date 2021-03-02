using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public State currentState;
    [Tooltip("This is the default state called RemainInState. If the decision object decides it does not need to change states, it will return this state")]
    public State remainInState;

    private bool aiActive = true;
    private float totalTimeInState;
    private float countdownTimeDelta;
    // Start is called before the first frame update
    private void Awake()
    {
        print("AI Starting");
        totalTimeInState = 0.0F;
        countdownTimeDelta = 0.0F;
    }

    // Update is called once per frame
    void Update()
    {
        if (!aiActive) return;
        
        currentState.UpdateState(this);

        totalTimeInState += Time.deltaTime;
    }

    public void TransitionToState(State nextState)
    {
        if (nextState != remainInState)
        {
            currentState = nextState;
            OnStateLeave();
        }
    }

    private void OnStateLeave()
    {
        // things to do when we are switching states
        countdownTimeDelta = 0.0F;
        totalTimeInState = 0.0F;
    }

    public float TimeInCurrentState()
    {
        return totalTimeInState;
    }

    public bool CheckIfCountdownElapsed(float duration)
    {
        countdownTimeDelta += Time.deltaTime;
        return (countdownTimeDelta >= duration);
    }
}
