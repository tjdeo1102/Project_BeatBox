using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FindCamera", story: "Find [Cam]", category: "Action", id: "49f7a7ac0221e4a48e17b18c7bade454")]
public partial class FindCameraAction : Action
{
    [SerializeReference] public BlackboardVariable<Camera> Cam;

    protected override Status OnStart()
    {
        if (Cam.Value != null) return Status.Success;

        Cam.Value = Camera.main;
        if (Cam.Value == null) return Status.Failure;
        else return Status.Success;
    }

}

