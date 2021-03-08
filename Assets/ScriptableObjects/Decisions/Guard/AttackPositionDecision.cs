using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/Guard/In Attack Position")]
public class AttackPositionDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        GuardStateController gsc = (GuardStateController) controller;
        Vector2 dis = gsc.chaseTarget.transform.position - gsc.transform.position;

        return Mathf.Abs(dis.x) <= 2.5 && !gsc.guardAttack1.performingAttack;
    }
}
