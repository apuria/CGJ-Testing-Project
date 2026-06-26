using System;
using System.Collections.Generic;
using UniFramework.Machine;
using UnityEngine;

/// <summary>
/// 战斗开始时做什么的枚举
/// </summary>
public enum EBattleStartEvent
{
    /// <summary>
    /// 无特殊事件，直接开始战斗
    /// </summary>
    None,
    /// <summary>
    /// 播放对话
    /// </summary>
    PlayDialogue,
}

/// <summary>
/// 战斗结束时做什么的枚举
/// </summary>
public enum EBattleEndEvent
{
    /// <summary>
    /// 无特殊事件
    /// </summary>
    None,
    /// <summary>
    /// 下一个对话
    /// </summary>
    NextDialogue,
    /// <summary>
    /// 回到上一个状态
    /// </summary>
    GoBackToLastState,
    /// <summary>
    /// 开始分支选项
    /// </summary>
    StartBranch,
    /// <summary>
    /// 回到地图
    /// </summary>
    GoBackToMap,
}

/// <summary>
/// 中途事件触发条件类型
/// </summary>
public enum EMidEventTriggerType
{
    /// <summary>
    /// 角色血量跌到指定百分比时触发
    /// </summary>
    CharacterHpThreshold,
    /// <summary>
    /// 敌人血量跌到指定百分比时触发
    /// </summary>
    EnemyHpThreshold,
    /// <summary>
    /// 到达指定回合时触发
    /// </summary>
    TurnCount,
}

/// <summary>
/// 中途事件触发后做什么
/// </summary>
public enum EMidEventAction
{
    /// <summary>
    /// 播放对话
    /// </summary>
    PlayDialogue,
    /// <summary>
    /// 开始分支选项
    /// </summary>
    StartBranch,
}

/// <summary>
/// 敌人配置
/// </summary>
[Serializable]
public class EnemyConfig
{
    [Tooltip("敌人角色ID")]
    public int enemyRoleIndex;
}

/// <summary>
/// 中途事件配置
/// </summary>
[Serializable]
public class MidBattleEvent
{
    [Tooltip("触发条件类型")]
    public EMidEventTriggerType triggerType = EMidEventTriggerType.CharacterHpThreshold;

    [Tooltip("目标索引\n- CharacterHpThreshold: 角色索引\n- EnemyHpThreshold: 敌人索引\n- TurnCount: 不使用此字段")]
    public int targetIndex;

    [Tooltip("血量百分比阈值（0~100），仅在 CharacterHpThreshold / EnemyHpThreshold 时使用")]
    [Range(0, 100)]
    public int hpPercentage = 50;

    [Tooltip("第几回合触发，仅在 TurnCount 时使用")]
    public int turnCount = 1;

    [Tooltip("触发后做什么")]
    public EMidEventAction action = EMidEventAction.PlayDialogue;

    [Tooltip("触发后播放的对话（action = PlayDialogue 时使用）")]
    public DialogueSettings dialogue;

    [Tooltip("触发后开启的分支（action = StartBranch 时使用）")]
    public BranchSetting branch;
}

[CreateAssetMenu(fileName = "New Battle Setting", menuName = "Battle/New Battle Setting")]
public class BattleSetting : ScriptableObject, IStateData
{
    //TODO:
    /*
    1. 战斗场景
    2. 敌人列表 (最多三个)
    3. 开始事件, 开始做什么的枚举
    4. 结束事件, 结束做什么的枚举
    5. 中途满足特定条件的事件, 做什么的枚举
    */
    

#region 事件
    [Header("战斗场景")]
    [Tooltip("战斗场景名称")]
    public string battleSceneName;

    [Tooltip("敌人列表（最多三个）")]
    public List<EnemyConfig> enemies = new();

    [Header("开始事件")]
    [Tooltip("战斗开始时做什么")]
    public EBattleStartEvent startEvent = EBattleStartEvent.None;

    [Tooltip("开始事件为 PlayDialogue 时使用的对话配置")]
    public DialogueSettings startDialogue;

    [Header("结束事件")]
    [Tooltip("战斗结束时做什么")]
    public EBattleEndEvent endEvent = EBattleEndEvent.None;

    [Tooltip("结束事件为 NextDialogue 时使用的对话配置")]
    public DialogueSettings endDialogue;

    [Tooltip("结束事件为 StartBranch 时使用的分支配置")]
    public BranchSetting endBranch;

    [Header("中途事件")]
    [Tooltip("战斗中满足特定条件时触发的事件列表")]
    public List<MidBattleEvent> midEvents = new();
#endregion
}
