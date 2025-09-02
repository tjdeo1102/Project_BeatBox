using System;
using Unity.Behavior;

public enum StateType
{
    Pause, Play, Jump, AttackRed, AttackGreen, AttackBlue
}

public enum ColorType
{
    Red, Green, Blue
}

public enum HitJudgeMode
{
    FirstHit, ComboHIt
}

public enum LayerType
{
    Player = 6, 
    Note = 7,
    PlayerAttack = 8,
    Ground = 9,
}

public enum SceneIndex
{
    Login, Loading, Lobby ,InGame, None
}

[BlackboardEnum]
public enum BossState
{
    Idle, Attack, Die
}