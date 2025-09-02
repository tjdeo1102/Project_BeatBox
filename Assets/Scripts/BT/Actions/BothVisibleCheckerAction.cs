using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Reflection;
using Unity.VisualScripting;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BothVisibleChecker", story: "[Player] and [Boss] are Visible on [Screen]", category: "Action", id: "8decc8298f5d7d8a9fe1edbfb07f517e")]
public partial class BothVisibleCheckerAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<GameObject> Boss;
    [SerializeReference] public BlackboardVariable<Camera> Screen;

    protected override Status OnStart()
    {
        if (Player.Value == null || Boss.Value == null || Screen.Value == null) return Status.Failure;
        if (!IsFullyVisible(Player.Value) || !IsFullyVisible(Boss.Value)) return Status.Failure;
        if (!TryStopPlayer()) return Status.Failure;

        else return Status.Success;
    }


    private bool IsFullyVisible(GameObject obj)
    {
        var rend = obj.GetComponentInChildren<Renderer>();
        if (rend == null) return false;

        Bounds bounds = rend.bounds;
        Vector3 min = Screen.Value.WorldToViewportPoint(bounds.min);
        Vector3 max = Screen.Value.WorldToViewportPoint(bounds.max);

        // 전체가 뷰포트 안 (0~1)이어야 함
        return min.x >= 0 && min.y >= 0 && max.x <= 1 && max.y <= 1 && min.z > 0;
    }

    private bool TryStopPlayer()
    {
        if (Player.Value.TryGetComponent<PlayerController>(out var ctrl) == false) return false;
        var playState = ctrl.Machine.StateDic[StateType.Play] as PlayState;
        if (playState == null) return false;
        
        playState.Speed = 0;
        return true;
    }
}

