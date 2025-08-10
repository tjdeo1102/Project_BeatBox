using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Require Setting")]
    public PlayerInput Input;
    public PlayerStateMachine Machine;
    public PlayerPhysics Physics;
    public BackgroundShift BackgroundShift;
    public Rigidbody2D Rigid;

    [Header("BlackBoard")]
    public bool IsCombo = false;
    public float ComboHitTimer = 0f;
    public NoteGroup CurrentComboNote;

    #region Test
    public bool TestPlayCheck;
    public void Start()
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
            Machine.ChangeState(StateType.Jump);
        }
    }
    public void OnAttackRed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Machine.ChangeState(StateType.AttackRed);
        }
    }

    public void OnAttackGreen(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Machine.ChangeState(StateType.AttackGreen);
        }
    }

    public void OnAttackBlue(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Machine.ChangeState(StateType.AttackBlue);
        }
    }

    //public void OnAttackRed()
    //{
    //    if (Machine.CanOtherAction())
    //        Machine.ChangeState(StateType.AttackRed);
    //}
    //public void OnAttackGreen()
    //{
    //    if (Machine.CanOtherAction())
    //        Machine.ChangeState(StateType.AttackGreen);
    //}
    //public void OnAttackBlue()
    //{
    //    if (Machine.CanOtherAction())
    //        Machine.ChangeState(StateType.AttackBlue);
    //}
    #endregion
}
