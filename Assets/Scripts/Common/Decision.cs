using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decision")]
public abstract class Decision : ScriptableObject
{
    public abstract bool Decide(StateController controller);
}
