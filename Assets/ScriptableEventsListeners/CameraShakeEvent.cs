using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Events/Camera/Shake")]
public class CameraShakeEvent : ScriptableObject
{
    private List<CameraShakeEventListener> subs = new List<CameraShakeEventListener>();

    public void Raise(float intensity, float time)
    {
        for (int i = subs.Count - 1; i >= 0; --i)
        {
            subs[i].OnEventRaised(intensity, time);
        }
    }

    public void register(CameraShakeEventListener listener)
    {
        subs.Add(listener);
    }

    public void unregister(CameraShakeEventListener listener)
    {
        subs.Remove(listener);
    }
}
