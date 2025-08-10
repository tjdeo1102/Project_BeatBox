using UnityEngine;

[CreateAssetMenu(menuName = "States/Jump")]
public class JumpState : State
{
    [Header("Require Setting")]
    public float JumpPower;
    public float RaycastDistance;

    public override void Enter()
    {
        base.Enter();

        if (CanJump())
            ctrl.Rigid.linearVelocityY = JumpPower;
        ctrl.Machine.ChangeState(StateType.Play);
    }

    public bool CanJump()
    {
        return Physics2D.Raycast(ctrl.transform.position, -ctrl.transform.up, RaycastDistance, 1 << (int)LayerType.Ground);
    }    
}
