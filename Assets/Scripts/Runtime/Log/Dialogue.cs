using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EOnEnd
{
    /// <summary>
    /// 下一个对话
    /// </summary>
    NextDialogue,
    /// <summary>
    /// 开启战斗
    /// </summary>
    StartBattle,
    /// <summary>
    /// 回到上一个状态
    /// </summary>
    GoBackToLastState,
    ///　<summary>
    /// 开始分支选项
    /// </summary>
    StartBranch,
}

public enum TalkingSpeaker
{
    Left,
    Right,
}


[Serializable]
public class Dialogue
{
    [Tooltip("对话角色ID, -1则没有, 不要超过角色数量")]
    public int leftSpeakerIndex = -1;
    [Tooltip("对话角色ID, -1则没有, 不要超过角色数量")]
    public int rightSpeakerIndex = -1;
    public TalkingSpeaker talkingSpeaker = TalkingSpeaker.Left;

    [Tooltip("说话文本")]
    [TextArea(5, 10)]
    public string text;

    

    public EOnEnd onEnd = EOnEnd.NextDialogue;
    public BattleSetting Battle;

    public BranchSetting BranchDialogue;

}
