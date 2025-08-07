using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Require Setting")]
    public PlayerInput Input;
    public PlayerStateMachine Machine;
    public Rigidbody2D Rigid;

    [Header("Player Setting")]
    public float Speed;

    #region Test
    public bool TestPlayCheck;
    public void Update()
    {
        Play(TestPlayCheck);
    }
    #endregion

    #region PlayerGameLoop
    public void Play(bool isPlay)
    {
        if (isPlay)
        {
            Rigid.linearVelocity = Vector2.right * Speed;
        }
        else
        {
            Rigid.linearVelocity = Vector2.zero;
        }
    }
    #endregion

    #region PlayerInputControl
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Machine.ChangeState(StateType.Jump);
        }
    }
    public void OnAttackRed()
    {
        Machine.ChangeState(StateType.AttackRed);
    }
    public void OnAttackGreen()
    {
        Machine.ChangeState(StateType.AttackGreen);
    }
    public void OnAttackBlue()
    {
        Machine.ChangeState(StateType.AttackBlue);
    }
    #endregion
}
