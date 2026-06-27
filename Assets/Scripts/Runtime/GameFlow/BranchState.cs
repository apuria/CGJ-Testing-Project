using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Machine;
using UniFramework.Event;

public class BranchState : BaseState
{

//TODO: 分支系统
/*
先初始化UI界面
随后由UI发送消息, 让这个状态判断玩家的选择
然后根据配置, 让状态机继续运行
*/

    public BranchSetting branchSetting;
    
    public override void OnCreate(StateMachine machine, IStateData data)
    {
        base.OnCreate(machine, data);
        branchSetting = data as BranchSetting;
    }

    public override void OnEnter()
    {
        eventGroup.AddListener<BranchEventDefine.ChooseBranch>(OnHandleEventMessage);
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnHandleEventMessage(IEventMessage message)
    {
        if(message is BranchEventDefine.ChooseBranch chooseBranch)
        {
            GameManager.Instance.AddBranchChoose(branchSetting.id, branchSetting.branches[chooseBranch.branchId].chosen);
        }
    }

    public void Continue()
    {
        // 先判断分支选择结束后应该做什么
        switch(branchSetting.onEnd)
        {
            case EOnEnd.NextDialogue:
                // 进入下一个对话
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
                // 开始新的分支选项
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
        // 使用状态机切换到对话状态
        StateEventDefine.ChangeState.SendEventMessage<DialogueState>("LogState", branchSetting.nextDialogue);
    }

    private void StartBattle()
    {
        // 使用状态机切换到战斗状态
        StateEventDefine.ChangeState.SendEventMessage<BattleState>("BattleState", branchSetting.battleSetting);
    }

    private void GoBackToLastState()
    {
        // 使用状态机切换到上一个状态
        StateEventDefine.BackToPrevState.SendEventMessage();
    }

    private void StartBranch()
    {
        // 使用状态机切换到分支选项状态
        StateEventDefine.ChangeState.SendEventMessage<BranchState>("BranchState", branchSetting.nextBranch);
    }

    private void GoBackToMap()
    {
        // 使用状态机切换到地图状态
        StateEventDefine.ChangeState.SendEventMessage<MapState>("MapState");
    }
}
