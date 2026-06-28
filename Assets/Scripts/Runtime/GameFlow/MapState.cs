using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Machine;
using UniFramework.Event;

public class MapState : BaseState
{
//TODO: 地图状态
/*
1. 监听地图UI的开启按钮，点击后发送消息，跳转节点事件
*/
    PlayerData playerData;
    public override void OnCreate(StateMachine machine, IStateData data)
    {
        base.OnCreate(machine, data);
        UIMgr.Instance.ShowPanel<MapPanel>(isSync: true);
    }

    public override void OnEnter()
    {
        
        playerData = GameManager.Instance.PlayerData;    
        switch(playerData.NowFlow)
        {
            case PlayerData.Flow.Start:
                //发送消息，跳转开始事件
                SceneEventDefine.StartGame.SendEventMessage();
                break;
            case PlayerData.Flow.End:
                //发送消息，跳转结束事件
                SceneEventDefine.EndGame.SendEventMessage();
                break;
            default:
                //TODO：更新地图UI
                break;
        }
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {

    }

    public override void OnDispose()
    {
        UIMgr.Instance.HidePanel<MapPanel>(true);
        base.OnDispose();
    }

    public override void OnHandleEventMessage(IEventMessage message)
    {
        
    }

}
