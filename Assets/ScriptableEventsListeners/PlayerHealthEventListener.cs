using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealthEventListener : MonoBehaviour
{
    public PlayerHealthEvent Event;
    public UnityEvent Response;

    private void OnEnable()
    {
        Event.register(this);
    }

    private void OnDisable()
    {
        Event.unregister(this);
    }

    public void OnEventRaised()
    {
        Response.Invoke();
    }
}
