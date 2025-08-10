using UnityEngine;

[CreateAssetMenu(menuName = "States/Pause")]
public class PauseState : State
{
    public override void Enter()
    {
        base.Enter();
        Time.timeScale = 0f;
    }
}
