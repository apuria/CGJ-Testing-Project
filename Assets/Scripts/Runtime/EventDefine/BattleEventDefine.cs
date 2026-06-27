using System.Collections;
using System.Collections.Generic;
using UniFramework.Event;
using UnityEngine;

public class BattleEventDefine : MonoBehaviour
{
//TODO: 战斗事件
/*
1. 指定角色受到伤害
2. 指定敌人受到伤害
3. 更新UI
4. 战斗结束
*/

    public class NextTurn : IEventMessage
    {
        public static void SendEventMessage()
        {
            var msg = new NextTurn();
            UniEvent.SendMessage(msg);
        }
    }

    public class NextRound : IEventMessage
    {
        public static void SendEventMessage()
        {
            var msg = new NextRound();
            UniEvent.SendMessage(msg);
        }
    }

    public class EnemyHpChange : IEventMessage
    {
        public int idx;
        public int hurtValue;
        public static void SendEventMessage(int idx, int hurtValue)
        {
            var msg = new EnemyHpChange();
            msg.idx = idx;
            msg.hurtValue = hurtValue;
            UniEvent.SendMessage(msg);
        }
    }

    public class RoleHpChange : IEventMessage
    {
        public int idx;
        public int hurtValue;
        public static void SendEventMessage(int idx, int hurtValue)
        {
            var msg = new RoleHpChange();
            msg.idx = idx;
            msg.hurtValue = hurtValue;
            UniEvent.SendMessage(msg);
        }
    }


    
}
