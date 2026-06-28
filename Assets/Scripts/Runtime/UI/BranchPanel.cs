using System.Collections;
using System.Collections.Generic;
using UniFramework.Event;
using UnityEngine;
using UnityEngine.UI;

public class BranchPanel : BasePanel
{
//TODO:
/*
1. 显示分支选项UI
2. 玩家选择分支后发送 ChooseBranch 事件
*/

#region 属性



#endregion

#region 生命周期

    protected void Awake()
    {
        eventGroup = new();
    }

    protected void Start()
    {

    }

    protected void OnDestroy()
    {
        eventGroup.RemoveAllListener();
    }
#endregion

#region 逻辑控制

    public override void HideMe()
    {

    }

    public override void ShowMe()
    {

    }

#endregion

#region 事件监听

    public void OnHandleEventMessage(IEventMessage message)
    {

    }

#endregion
}
