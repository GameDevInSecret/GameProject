using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/Guard/Look")]
public class LookDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        // This might be really expensive
        GuardStateController gsc = (GuardStateController) controller;
        
        Vector2 origin = gsc.guardPoint;
        origin.x -= gsc.activiationRange;

        bool playerInRange = Physics2D.Raycast(origin, Vector2.right, gsc.activiationRange * 2, gsc.lookLayers);

        return playerInRange;
    }
}
