using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Machine;

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

        

    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnHandleEventMessage()
    {
        
    }

    public void Continue()
    {
        // 先判断分支选择结束后应该做什么
        switch(branchSetting.onEnd)
        {
            case EOnEnd.NextDialogue:
                // 进入下一个对话
                break;
            case EOnEnd.StartBattle:
                // 开启战斗
                break;
            case EOnEnd.GoBackToLastState:
                // 回到上一个状态
                break;
            case EOnEnd.StartBranch:
                // 开始新的分支选项
                break;
            case EOnEnd.GoBackToMap:
                // 回到地图
                //TODO: 状态机清空所有挂起状态
                //TODO: 状态机回到地图状态
                break;
        }
    }


}
