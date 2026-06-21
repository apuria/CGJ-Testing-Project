using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Machine;
public class LogState: BaseState
{

    //进入对话状态，播放对话
    public DialogueSettings nowDialogue;

    public bool canGoNext = false;

    public int index = 0;

    public override void OnCreate(StateMachine machine, IStateData data)
    {
        base.OnCreate(machine, data);
        nowDialogue = data as DialogueSettings;
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
        //TODO: 使用状态机切换到战斗状态, 而且要使用删除当前状态的切换状态
    }

    private void GoBackToLastState()
    {
        //TODO: 使用状态机切换到上一个状态
    }

    private void StartBranch()
    {
        //TODO: 使用状态机切换到分支选项状态
    }
}
