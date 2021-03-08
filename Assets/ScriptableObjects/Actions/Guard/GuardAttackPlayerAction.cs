using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Guard/Attack Player")]
public class GuardAttackPlayerAction : Action
{
    public override void Act(StateController controller)
    {
        GuardStateController gsc = (GuardStateController) controller;
        if (!gsc.guardAttack1.startedAttack)
        {
            gsc.guardAttack1.StartAttack();
        }
    }
}
