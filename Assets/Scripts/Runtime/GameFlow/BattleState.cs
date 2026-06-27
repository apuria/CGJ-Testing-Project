using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Machine;
using UniFramework.Event;

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

    public int round = 0;

    public bool isBattleStart = false;

    public BattleSetting battleSetting;

    public StateMachine battleMachine;

    /// <summary>
    /// 战斗回合队列
    /// </summary>
    public Queue<BaseInfo> roundQueue;
    public List<BaseInfo> roundList;
    
    /// <summary>
    /// 角色列表
    /// </summary>
    public List<RoleInfo> roleList;
    public int liveRoleCount => roleList.FindAll(r => r.hp > 0).Count;
    /// <summary>
    /// 敌人列表
    /// </summary>
    public List<EnemyInfo> enemyList;
    public int liveEnemyCount => enemyList.FindAll(e => e.hp > 0).Count;

#region 生命周期
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
        StartEvent(battleSetting.startEvent);

        eventGroup.AddListener<BattleEventDefine.NextTurn>(OnHandleEventMessage);
        eventGroup.AddListener<BattleEventDefine.NextRound>(OnHandleEventMessage);
        eventGroup.AddListener<BattleEventDefine.EnemyHpChange>(OnHandleEventMessage);
        eventGroup.AddListener<BattleEventDefine.RoleHpChange>(OnHandleEventMessage);
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        battleMachine.Update();
    }

#endregion

#region 事件监听

    public override void OnHandleEventMessage(IEventMessage message)
    {
        if(message is BattleEventDefine.NextTurn)
        {
            NextTurn();
        }
        else if(message is BattleEventDefine.NextRound)
        {
            NextRound();
        }
        else if(message is BattleEventDefine.EnemyHpChange ehcMsg)
        {
            //TODO: 处理敌人血量变化事件
            /*
            1. 判断敌人是否死亡
            2. 如果敌人死亡，切换到敌人死亡状态
            3. 如果敌人未死亡，更新敌人血量
            */
        }
        else if(message is BattleEventDefine.RoleHpChange rhcMsg)
        {
            //TODO: 处理角色血量变化事件
            /*
            1. 判断角色是否死亡
            2. 如果角色死亡，切换到角色死亡状态
            3. 如果角色未死亡，更新角色血量
            */
        }
    }

#endregion

#region 游戏逻辑
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
        foreach (var enemy in battleSetting.enemies)
        {
            enemyList.Add(MonoMgr.Instantiate(enemy));
        }

        roleList.Add(MonoMgr.Instantiate(GameManager.Instance.mainRole));
        foreach(var roleBranch in battleSetting.moreRoleBranches)
        {
            if(GameManager.Instance.PlayerData.branchList[roleBranch.branchId] == roleBranch.choose)
            {
                roleList.Add(MonoMgr.Instantiate(GameManager.Instance.roleList[roleBranch.roleIndex]));
            }
        }

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

        CheckMidEvent();

        if(roundQueue.Count == 0)
        {
            NextRound();
            return;
        }

        BaseInfo info = roundQueue.Dequeue();
        if(info is RoleInfo roleInfo)
        {
            battleMachine.SwitchTo<PlayerBattleState>("PlayerBattleState", new PlayerBattleInfo(roleInfo));
        }
        else if(info is EnemyInfo enemyInfo)
        {
           battleMachine.SwitchTo<EnemyBattleState>("EnemyBattleState", new EnemyBattleInfo(enemyInfo)); 
        }
        
        //更新UI
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
        round++;
        CheckMidEvent();
        roundQueue.Clear();
        roundList.Clear();
        foreach (var role in roleList)
        {
            if(role.hp > 0)
                roundList.Add(role);
        }
        foreach (var enemy in enemyList)
        {
            if(enemy.hp > 0)
            {
                roundList.Add(enemy);
            }
        }

        // 按speed从大到小排序
        roundList.Sort((a, b) => b.speed.CompareTo(a.speed));
        // 将排序后的元素加入回合队列
        foreach (var info in roundList)
        {
            roundQueue.Enqueue(info);
        }
    }

#endregion

#region 事件处理

    public void CheckIsEnd()
    {
        //TODO: 检查战斗是否结束
        /*
        1. 检查存活的角色数量是否为0
        2. 如果角色数量为0，战斗失败
        3. 检查存活的敌人数量是否为0
        4. 如果敌人数量为0，战斗胜利
        */
        if(liveRoleCount == 0)
        {
            //TODO: 处理战斗失败事件
            /*
            1. 让UI显示战斗失败
            */
        }
        if(liveEnemyCount == 0)
        {
            //TODO: 处理战斗胜利事件
            /*
            1. 让UI显示战斗胜利
            */
        }
    }

    public void CheckMidEvent()
    {
        //TODO: 检查战斗中途事件
        /*
        1. 检查是否满足战斗中途事件的条件
        2. 如果满足，调用MidEvent()
        */

        foreach (var midEvent in battleSetting.midEvents)
        {
            switch(midEvent.triggerType)
            {
                case EMidEventTriggerType.CharacterHpThreshold:
                    // 检查角色血量是否达到阈值
                    if (roleList[midEvent.targetIndex].hp <= roleList[midEvent.targetIndex].maxHp * midEvent.hpPercentage * 0.01f)
                    {
                        MidEvent(midEvent);
                    }
                    break;
                case EMidEventTriggerType.EnemyHpThreshold:
                    // 检查敌人血量是否达到阈值
                    if (enemyList[midEvent.targetIndex].hp <= enemyList[midEvent.targetIndex].maxHp * midEvent.hpPercentage * 0.01f)
                    {
                        MidEvent(midEvent);
                    }
                    break;
                case EMidEventTriggerType.RoundCount:
                    // 检查回合数是否达到阈值
                    if(round == midEvent.roundCount)
                    {
                        MidEvent(midEvent);
                    }
                    break;
            }
        }
    }
    public void StartEvent(EBattleStartEvent _event)
    {
        switch(_event)
        {
            case EBattleStartEvent.None:
                // 无事件
                break;
            case EBattleStartEvent.PlayDialogue:
                // 播放对话
                StartDialogue();
                break;
            case EBattleStartEvent.StartBranch:
                // 开始分支选项
                StartBranchForStartEvent();
                break;
        }
    }

    public void WinEvent()
    {
        switch(battleSetting.winEvent)
        {
            case EBattleEndEvent.None:
                // 无事件
                break;
            case EBattleEndEvent.NextDialogue:
                // 进入下一个对话
                NextDialogue(battleSetting.winDialogue);
                break;
            case EBattleEndEvent.GoBackToLastState:
                // 回到上一个状态
                GoBackToLastState();
                break;
            case EBattleEndEvent.StartBranch:
                // 开始新的分支选项
                StartBranch(battleSetting.winBranch);
                break;
            case EBattleEndEvent.GoBackToMap:
                // 回到地图
                GoBackToMap();
                break;
        }
    }

    public void LoseEvent()
    {
        switch(battleSetting.loseEvent)
        {
            case EBattleEndEvent.None:
                // 无事件
                break;
            case EBattleEndEvent.NextDialogue:
                // 进入下一个对话
                NextDialogue(battleSetting.loseDialogue);
                break;
            case EBattleEndEvent.GoBackToLastState:
                // 回到上一个状态
                GoBackToLastState();
                break;
            case EBattleEndEvent.StartBranch:
                // 开始新的分支选项
                StartBranch(battleSetting.loseBranch);
                break;
            case EBattleEndEvent.GoBackToMap:
                // 回到地图
                GoBackToMap();
                break;
        }
    }

    public void MidEvent(MidBattleEvent midEvent)
    {
        switch (midEvent.action)
        {
            case EMidEventAction.PlayDialogue:
                // 播放对话
                MidDialogue(midEvent);
                break;
            case EMidEventAction.StartBranch:
                // 开始新的分支选项
                MidBranch(midEvent);
                break;
        }
    }


    private void StartDialogue()
    {
        // 使用状态机切换到对话状态，播放战斗开始对话
        StateEventDefine.ChangeState.SendEventMessage<DialogueState>("LogState", battleSetting.startDialogue,false);
    }

    private void StartBranchForStartEvent()
    {
        // 使用状态机切换到分支选项状态
        StateEventDefine.ChangeState.SendEventMessage<BranchState>("BranchState", battleSetting.startBranch, false);
    }

    private void NextDialogue(DialogueSetting dialogue)
    {
        // 使用状态机切换到对话状态
        StateEventDefine.ChangeState.SendEventMessage<DialogueState>("LogState", dialogue);
    }

    private void GoBackToLastState()
    {
        // 使用状态机切换到上一个状态
        StateEventDefine.BackToPrevState.SendEventMessage();
    }

    private void StartBranch(BranchSetting branch)
    {
        // 使用状态机切换到分支选项状态
        StateEventDefine.ChangeState.SendEventMessage<BranchState>("BranchState", branch, false);
    }

    private void GoBackToMap()
    {
        // 使用状态机切换到地图状态
        StateEventDefine.ChangeState.SendEventMessage<MapState>("MapState");
    }

    private void MidDialogue(MidBattleEvent midEvent)
    {
        // 使用状态机切换到对话状态，播放战斗中对话
        StateEventDefine.ChangeState.SendEventMessage<DialogueState>("LogState", midEvent.dialogue, false);
    }

    private void MidBranch(MidBattleEvent midEvent)
    {
        // 使用状态机切换到分支选项状态
        StateEventDefine.ChangeState.SendEventMessage<BranchState>("BranchState", midEvent.branch, false);
    }

#endregion
}