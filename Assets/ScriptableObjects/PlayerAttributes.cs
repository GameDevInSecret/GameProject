using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerAttributes : ScriptableObject, ISerializationCallbackReceiver
{
    public int initialHealth = 100;
    public PlayerHealthEvent healthEvent;
    [NonSerialized] public int _rtHealth;

    public void OnAfterDeserialize()
    {
        _rtHealth = initialHealth;
    }

    public void OnBeforeSerialize()
    { }

    public int GetHealth()
    {
        return _rtHealth;
    }

    public int LowerHealth(int val)
    {
        _rtHealth -= val;
        // Debug.Log("HEALTH LOWERED TO " + _rtHealth + " (-" + val + ")");
        healthEvent.Raise();
        return _rtHealth; 
    }

    public int RaiseHealth(int val)
    {
        _rtHealth += val;
        // Debug.Log("HEALTH RAISE TO " + _rtHealth + " (+" + val + ")");
        healthEvent.Raise();
        return _rtHealth;
    }

    public void SetInitHealth(int val)
    {
        initialHealth = val;
        _rtHealth = val;
    }
}
