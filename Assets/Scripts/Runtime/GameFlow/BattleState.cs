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
5. 失败条件: 角色全部死亡
6. 胜利条件: 自行配置
    1. 敌人全部死亡
    2. 指定敌人死亡
    3. 指定回合数
7. 游戏结束事件差分: 战斗胜利和战斗失败

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

    public StateMachine battleMachine;

    /// <summary>
    /// 战斗回合队列
    /// </summary>
    public Queue<BaseInfo> roundQueue;
    
    /// <summary>
    /// 角色列表
    /// </summary>
    public List<RoleInfo> roleList;
    /// <summary>
    /// 敌人列表
    /// </summary>
    public List<EnemyInfo> enemyList;


    public override void OnCreate(StateMachine machine, IStateData data)
    {
        base.OnCreate(machine, data);
        battleSetting = data as BattleSetting;
        battleMachine = new StateMachine();
    }

    public override void OnEnter()
    {
        
        if(isBattleStart)
            return;
        isBattleStart = true;
        InitBattle();
        NextRound();
        //TODO: 战斗开始
        /*
        1. 检查是否有战斗开始事件
        */
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        battleMachine.Update();
    }

    public override void OnHandleEventMessage()
    {
        
    }

    /// <summary>
    /// 初始化战斗
    /// </summary>
    public void InitBattle()
    {
        //TODO: 初始化战斗
        /*
        1. 记录本次战斗的角色列表
        2. 记录本次战斗的敌人列表
        3. UI更新战斗信息
        */
    }

    /// <summary>
    /// 下一个角色或者敌人行动
    /// </summary>
    public void NextTurn()
    {
        //TODO: 下一个角色或者敌人行动
        /*
        1. 从回合队列中取出下一个角色或者敌人
        2. 判断是角色还是敌人
        3. 将战斗状态机切换到角色行动状态或者敌人行动状态
        4. 更新UI
        */
    }

    /// <summary>
    /// 下一个回合
    /// </summary>
    public void NextRound()
    {
        //TODO: 下一个回合
        /*
        1. 清空回合队列
        2. 计算角色和敌人的行动顺序
        3. 将角色和敌人添加到回合队列中
        4. 调用函数NextTurn()
        5. 更新UI
        */
    }

    public void Continue(EBattleEndEvent _event)
    {
        // 先判断分支选择结束后应该做什么
        switch(_event)
        {
            case EBattleEndEvent.NextDialogue:
                // 进入下一个对话
                break;
            case EBattleEndEvent.GoBackToLastState:
                // 回到上一个状态
                break;
            case EBattleEndEvent.StartBranch:
                // 开始新的分支选项
                break;
            case EBattleEndEvent.GoBackToMap:
                // 回到地图
                //TODO: 状态机清空所有挂起状态
                //TODO: 状态机回到地图状态
                break;
        }
    }

}