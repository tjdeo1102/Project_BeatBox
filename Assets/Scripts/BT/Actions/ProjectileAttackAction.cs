using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Collections.Generic;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ProjectileAttack", story: "[Agent] Shoot [Target] with [Projectile]", category: "Action", id: "c0476be602922775148e944f83f9e878")]
public partial class ProjectileAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> Speed;
    [SerializeReference] public BlackboardVariable<List<GameObject>> ProjectileSpawn;
    // 임시로 특정 탄환만 발사되도록 구현
    [SerializeReference] public BlackboardVariable<string> Projectile;

    protected override Status OnStart()
    {
        if (CanUseProjectile(out var projectile))
        {
            var dir = (Target.Value.transform.position - Agent.Value.transform.position).normalized;
            projectile.Velocity = dir.x * Speed * Vector2.right;
            return Status.Success;
        }
        else return Status.Failure;
    }

    private bool CanUseProjectile(out Projectile projectTile)
    {
        projectTile = null;
        if (ObjectPoolManager.Instance == null) return false;
        var obj = ObjectPoolManager.Instance.GetObject(Projectile.Value);
        if (obj == null) return false;
        if (ProjectileSpawn.Value.Count < 1) return false;
        var posIdx = UnityEngine.Random.Range(0, ProjectileSpawn.Value.Count);

        if (obj.TryGetComponent<NoteGroup>(out var group))
        {
            group.OriginGroupPosition = ProjectileSpawn.Value[posIdx].transform.position;
            group.Init();
        }
        else obj.transform.position = ProjectileSpawn.Value[posIdx].transform.position;

        return obj.TryGetComponent<Projectile>(out projectTile);
    }
}

