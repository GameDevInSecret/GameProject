using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraShakeEventListener : MonoBehaviour
{

    public CameraShakeEvent Event;
    public C_CameraShakeEvent Response;
    
    private void OnEnable()
    {
        Event.register(this);
    }

    private void OnDisable()
    {
        Event.unregister(this);
    }

    public void OnEventRaised(float intensity, float time)
    {
        Response.Invoke(intensity, time);
    }
}
