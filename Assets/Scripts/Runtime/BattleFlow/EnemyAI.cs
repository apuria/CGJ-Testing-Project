using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI
{
//TODO:
/*
1. 计算敌人的行动
2. 返回敌人的行动
*/
    public EnemyActionType actionType;

    public static EnemyAI DoTurn(EnemyBattleInfo info)
    {
        EnemyAI enemyAI = new EnemyAI();

        //TODO: 计算敌人的行动
        /*
        1~2回合内, 随机选择攻击或防御
        2回合后, 敌人使用技能的机率慢慢增加
        使用技能后, 技能使用机率降低为一开始
        之后, 技能使用机率慢慢增加
        敌人使用技能不会消耗蓝
        */

        return enemyAI;
    }
}
