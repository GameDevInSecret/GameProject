using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/Guard/Back In Place")]
public class BackInPlaceDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        // This might be really expensive
        GuardStateController gsc = (GuardStateController) controller;
        
        float bound = gsc.returnBound;
        float upperBound = gsc.guardPoint.x + bound;
        float lowerBound = gsc.guardPoint.x - bound;
        float guardXPos = gsc.transform.position.x;
        
        return guardXPos >= lowerBound && guardXPos <= upperBound;
    }
}
