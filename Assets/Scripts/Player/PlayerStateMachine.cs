using System.Collections.Generic;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    [Header("Require Setting")]
    public PlayerController Ctrl;
    public List<State> States;
    public StateType CurType;

    private Dictionary<StateType, State> stateDic;
    private State curState;

    void Start()
    {
        stateDic = new();
        foreach (var item in States)
        {
            item.Init(Ctrl);
            stateDic[item.Type] = item;
        }

        // 시작 상태 세팅
        if (stateDic.ContainsKey(StateType.Pause))
        {
            ChangeState(StateType.Pause);
        }
    }

    void Update()
    {
        curState?.Update();
    }

    public void ChangeState(StateType type)
    {
        curState?.Exit();
        CurType = type;
        curState = stateDic[CurType];
        curState?.Enter();
    }

    public bool CanOtherAction()
    {
        bool canAction = CurType == StateType.Pause
                        || CurType == StateType.Play
                        || CurType == StateType.Jump;
        return canAction;
    }
}
