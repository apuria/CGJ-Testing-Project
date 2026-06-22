using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Machine;

public class BranchState : BaseState
{
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
        //TODO: 玩家已经做出选择，继续游戏, 让状态机返回到上一个状态
    }


}
