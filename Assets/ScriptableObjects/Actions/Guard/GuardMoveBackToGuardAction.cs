using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Guard/Move Back To Guard Position")]
public class GuardMoveBackToGuardAction : Action
{
    public override void Act(StateController controller)
    {
        GuardStateController gsc = (GuardStateController) controller;

        if (!gsc.CheckIfCountdownElapsed(gsc.pauseTime)) return;

        Vector2 dir = (Vector3)gsc.guardPoint - gsc.chaseTarget.transform.position;
        gsc.transform.Translate(dir.normalized * gsc.speed * Time.deltaTime);
    }
}
