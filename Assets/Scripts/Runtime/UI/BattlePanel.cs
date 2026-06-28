using System.Collections;
using System.Collections.Generic;
using UniFramework.Event;
using UnityEngine;

public class BattlePanel : BasePanel
{
//TODO:
/*
1. 
*/

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
