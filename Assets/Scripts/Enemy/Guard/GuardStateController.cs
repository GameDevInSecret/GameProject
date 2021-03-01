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
