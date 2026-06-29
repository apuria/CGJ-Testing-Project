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
    public EnemyBattleInfo(EnemyInfo enemyInfo)
    {
        this.enemyInfo = enemyInfo;
    }
}
