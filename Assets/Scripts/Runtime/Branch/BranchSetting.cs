using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Machine;

[CreateAssetMenu(fileName = "New Branch", menuName = "Branch/New Branch")]
public class BranchSetting : ScriptableObject, IStateData
{
    [Tooltip("分支选项列表")]
    public List<Branch> branches;

    [Tooltip("选择结束后让状态机做什么")]
    public EOnEnd onEnd;

    [Tooltip("结束后的下一个对话（onEnd = NextDialogue 时使用）")]
    public DialogueSettings nextDialogue;

    [Tooltip("结束后开启的战斗配置（onEnd = StartBattle 时使用）")]
    public BattleSetting battleSetting;

    [Tooltip("结束后开启的分支配置（onEnd = StartBranch 时使用）")]
    public BranchSetting nextBranch;
}
