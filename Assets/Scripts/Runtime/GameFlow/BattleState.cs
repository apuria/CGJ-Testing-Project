using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Machine;

public class BattleState : BaseState
{

    public bool isBattleStart = false;

    public BattleSetting battleSetting;
    
    public override void OnCreate(StateMachine machine, IStateData data)
    {
        base.OnCreate(machine, data);
        battleSetting = data as BattleSetting;
    }

    public override void OnEnter()
    {
        
        if(isBattleStart)
            return;
        isBattleStart = true;
        //TODO: 初始化战斗
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        
    }
}
