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
        var gm = GameManager.Instance;
        switch(playerData.NowFlow)
        {
            case PlayerData.Flow.Start:
                if (gm.HasStartEvent)
                {
                    SceneEventDefine.StartGame.SendEventMessage();
                }
                else
                {
                    // 没有开始事件，直接跳到下一个节点，避免卡住
                    gm.NextNode();
                }
                break;
            case PlayerData.Flow.End:
                if (gm.HasEndEvent)
                {
                    SceneEventDefine.EndGame.SendEventMessage();
                }
                else
                {
                    // 没有结束事件，直接跳到下一个
                    gm.NextNode();
                }
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
