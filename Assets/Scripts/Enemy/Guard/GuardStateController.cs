using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardStateController : StateController
{
    public float speed;
    public float attackSpeed;
    public Vector2 guardPoint;
    public float activiationRange;
    public float pauseTime;
    public Transform chaseTarget;
    public LayerMask lookLayers;

    public float returnBound = 0.1F;
    public Damager _damager;

    public GuardAttack1 guardAttack1;

    public void Start()
    {
        _damager = GetComponent<Damager>();
        guardAttack1 = GetComponent<GuardAttack1>();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = currentState.stateColor;
        
        Vector2 from = guardPoint;
        from.x -= activiationRange;
        Vector2 to = guardPoint;
        to.x += activiationRange;
        
        Gizmos.DrawLine(from, to);
        Gizmos.DrawWireSphere(guardPoint, 0.1F);
    }
}
