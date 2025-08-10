using System;

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

