using System.Collections;
using System.Collections.Generic;
using UniFramework.Machine;
using UnityEngine;

public struct PlayerBattleInfo : IStateData
{
    RoleInfo roleInfo;
    public PlayerBattleInfo(RoleInfo roleInfo)
    {
        this.roleInfo = roleInfo;
    }
}

public struct EnemyBattleInfo : IStateData
{
    EnemyInfo enemyInfo;
    int round;
    public EnemyBattleInfo(EnemyInfo enemyInfo, int round)
    {
        this.enemyInfo = enemyInfo;
        this.round = round;
    }
}

public enum EnemyActionType
{
    Defence,
    Attack,
    Skill
}

