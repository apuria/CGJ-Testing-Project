using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerData
{
//TODO:
/*
1. 玩家所在游戏节点
2. 记录玩家关键选择
3. 可能还要记录当前玩家的状态
*/
    /// <summary>
    /// 玩家所在游戏节点
    /// 完成当前节点后，才会进入下一个节点
    /// </summary>
    enum Flow
    {
        Start = 0,
        //TODO:
        //更多的节点
    }

    private Flow nowFlow = Flow.Start;

    public string NowFlow => nowFlow.ToString();

    public void NextFlow()
    {
        nowFlow++;
    }

    

}
