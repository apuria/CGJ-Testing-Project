using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Machine;

public class MapState : BaseState
{
//TODO:
/*
1. 从游戏管理器中获取玩家配置
2. 如果玩家配置表示为新游戏，则跳转开始事件
3. 如果玩家配置表示为游戏进度中途，则更新地图, 并且监听玩家配置的变更
4. 如果玩家配置表示为游戏结束节点, 则跳转结束事件
*/
    
    public override void OnCreate(StateMachine machine, IStateData data)
    {
        base.OnCreate(machine, data);
    }

    public override void OnEnter()
    {
        PlayerData playerData = GameManager.Instance.PlayerData;    
        switch(playerData.NowFlow)
        {
            case PlayerData.Flow.Start:
                //TODO: 发送消息，跳转开始事件
                break;
            case PlayerData.Flow.Node1:
                //TODO: 更新地图
                break;
            case PlayerData.Flow.Node2:
                 //TODO: 更新地图
                break;
            case PlayerData.Flow.Node3:
                break;
            case PlayerData.Flow.Node4:
                break;
            case PlayerData.Flow.Node5:
                break;
            case PlayerData.Flow.Node6:
                break;
            case PlayerData.Flow.End:
                //TODO: 发送消息，跳转结束事件
                break;
        }
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
