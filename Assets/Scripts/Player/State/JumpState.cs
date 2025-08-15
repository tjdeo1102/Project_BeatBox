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
        return Physics2D.BoxCast(
            ctrl.transform.position,   
            Vector2.one,               
            0f,                        
            -ctrl.transform.up,        
            RaycastDistance,           
            1 << (int)LayerType.Ground
        );
    }    
}
