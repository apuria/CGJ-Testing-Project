using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Machine;
using UnityEngine.XR;
using UniFramework.Event;
public class DialogueState : BaseState
{

    //进入对话状态，播放对话
    public DialogueSetting nowDialogue;

    public bool canGoNext = false;

    public int index = 0;

    public override void OnCreate(StateMachine machine, IStateData data)
    {
        base.OnCreate(machine, data);
        nowDialogue = data as DialogueSetting;
    }

    public override void OnEnter()
    {
        //TODO: 更新对话面板
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        
    }

    public void Continue()
    {
        //先判断这个对话的下一个操作是什么，然后进行相应的操作
        switch(nowDialogue.dialogues[index].onEnd)
        {
            case EOnEnd.NextDialogue:
                // 播放下一条对话
                NextDialogue();
                break;
            case EOnEnd.StartBattle:
                // 开启战斗
                StartBattle();
                break;
            case EOnEnd.GoBackToLastState:
                // 回到上一个状态
                GoBackToLastState();
                break;
            case EOnEnd.StartBranch:
                // 开始分支选项
                StartBranch();
                break;
            case EOnEnd.GoBackToMap:
                // 回到地图
                GoBackToMap();
                break;
        }
    }

    private void NextDialogue()
    {
        //如果是最后一个对话，则回到上一个状态
        if(index == nowDialogue.dialogues.Count - 1)
        {
            GoBackToLastState();
        }
        else
        {
            index++;
            //TODO: 更新对话面板
        }
    }

    private void StartBattle()
    {
        //使用状态机切换到战斗状态, 而且要使用删除当前状态的切换状态
        StateEventDefine.ChangeState.SendEventMessage<BattleState>("BattleState", nowDialogue.dialogues[index].Battle);
    }

    private void GoBackToLastState()
    {
        //使用状态机切换到上一个状态
        StateEventDefine.BackToPrevState.SendEventMessage();
    }

    private void StartBranch()
    {
        //使用状态机切换到分支选项状态
        StateEventDefine.ChangeState.SendEventMessage<BranchState>("BranchState", nowDialogue.dialogues[index].Branch, false);
    }

    private void GoBackToMap()
    {
        //使用状态机切换到地图状态
        StateEventDefine.ChangeState.SendEventMessage<MapState>("MapState");
    }

    public override void OnHandleEventMessage(IEventMessage message)
    {
        
    }
}
