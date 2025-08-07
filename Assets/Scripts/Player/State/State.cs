using System;
using UnityEngine;

public abstract class State: ScriptableObject
{
    public StateType Type;
    protected PlayerController ctrl;

    public void Init(PlayerController controller)
    {
        this.ctrl = controller;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}