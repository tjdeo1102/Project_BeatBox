using UnityEngine;

[CreateAssetMenu(menuName = "States/Play")]
public class PlayState : State
{
    [Header("Require Setting")]
    public float PlayTimeScale;
    public float Speed;

    private Rigidbody2D m_rigid;
    public override void Enter()
    {
        base.Enter();
        m_rigid = ctrl.Rigid;
        Time.timeScale = PlayTimeScale;
    }
    public override void Update()
    {
        base.Update();
        m_rigid.linearVelocity = new Vector2(Speed, m_rigid.linearVelocityY);
    }
    public override void Exit()
    {
        base.Exit();
    }
}
