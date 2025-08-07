using UnityEngine;

[CreateAssetMenu(menuName = "States/Jump")]
public class JumpState : State
{
    [Header("Require Setting")]
    public float JumpPower;

    public override void Enter()
    {
        base.Enter();
        ctrl.Rigid.linearVelocityY = JumpPower;
        ctrl.Machine.ChangeState(StateType.Play);
    }
}
