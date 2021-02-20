using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerHealthEvent : ScriptableObject
{
    private List<PlayerHealthEventListener> subs = new List<PlayerHealthEventListener>();

    public void Raise()
    {
        for (int i = subs.Count - 1; i >= 0; --i)
        {
            subs[i].OnEventRaised();
        }
    }

    public void register(PlayerHealthEventListener listener)
    {
        subs.Add(listener);
    }

    public void unregister(PlayerHealthEventListener listener)
    {
        subs.Remove(listener);
    }
}
