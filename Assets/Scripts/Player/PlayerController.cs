using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Require Setting")]
    public PlayerInput Input;
    public PlayerStateMachine Machine;
    public Rigidbody2D Rigid;

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
            Machine.ChangeState(StateType.Play);
        }
        else
        {
            Machine.ChangeState(StateType.Pause);
        }
    }
    #endregion

    #region PlayerInputControl
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        { 
            // 2단 점프 방지 로직 필요
            Machine.ChangeState(StateType.Jump);
        }
    }
    public void OnAttackRed()
    {
        if (Machine.CanOtherAction())
            Machine.ChangeState(StateType.AttackRed);
    }
    public void OnAttackGreen()
    {
        if (Machine.CanOtherAction())
            Machine.ChangeState(StateType.AttackGreen);
    }
    public void OnAttackBlue()
    {
        if (Machine.CanOtherAction())
            Machine.ChangeState(StateType.AttackBlue);
    }
    #endregion
}
