using UnityEngine;

[CreateAssetMenu(menuName = "States/Play")]
public class PlayState : State
{
    [Header("Require Setting")]
    public float PlayTimeScale;
    public float Speed;
    public override void Enter()
    {
        base.Enter();
        var rigid = ctrl.Rigid;
        rigid.linearVelocity = new Vector2(Speed, rigid.linearVelocityY);
        Time.timeScale = PlayTimeScale;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
