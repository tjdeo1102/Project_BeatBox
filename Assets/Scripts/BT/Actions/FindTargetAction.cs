using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FindTarget", story: "Find [Target]", category: "Action", id: "4ff407f21febc1da66267c50f30aa4a4")]
public partial class FindTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    protected override Status OnStart()
    {
        if (Target.Value != null) return Status.Success;
        
        var loop = InGameLoop.Instance;
        if (loop == null || loop.Player == null)
        {
            return Status.Failure;
        }
        Target.Value = loop.Player.gameObject;

        return Status.Success;
    }
}

