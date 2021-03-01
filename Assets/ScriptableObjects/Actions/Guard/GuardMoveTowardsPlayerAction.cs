using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Guard/Move Towards Player")]
public class GuardMoveTowardsPlayerAction : Action
{
    public override void Act(StateController controller)
    {
        GuardStateController gsc = (GuardStateController) controller;

        Vector2 dis = gsc.chaseTarget.transform.position - gsc.transform.position;
        Vector2 dir = new Vector2(dis.x, 0); // find which direction we need to move
        
        if (Mathf.Abs(dis.x) > 2)
        {
            gsc.transform.Translate( dir.normalized * gsc.attackSpeed * Time.deltaTime);
        }
    }
}
