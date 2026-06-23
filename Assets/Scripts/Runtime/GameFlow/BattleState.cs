using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Machine;

public class BattleState : BaseState
{
//TODO: 战斗状态
/*
1. 战斗状态机
2. 检查是否有战斗开始事件
3. 检查是否有战斗结束事件
4. 每次切换战斗状态时，检查是否满足战斗中途事件的条件

1. 从游戏管理器中读取玩家配置: 
    1. 有几个角色
    2. 角色信息
    3. 添加到战斗状态中
2. 从战斗配置表中读取敌人配置:
    1. 有几个敌人
    2. 敌人信息
    3. 添加到战斗状态中
*/

    public bool isBattleStart = false;

    public BattleSetting battleSetting;
    
    public override void OnCreate(StateMachine machine, IStateData data)
    {
        base.OnCreate(machine, data);
        battleSetting = data as BattleSetting;
    }

    public override void OnEnter()
    {
        
        if(isBattleStart)
            return;
        isBattleStart = true;
        //TODO: 初始化战斗
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

    
}