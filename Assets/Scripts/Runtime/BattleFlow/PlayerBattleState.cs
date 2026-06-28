using System.Collections;
using System.Collections.Generic;
using UniFramework.Machine;
using UnityEngine;

public class PlayerBattleState : BaseBattleState
{
    public PlayerBattleInfo info;
    public override void OnCreate(StateMachine machine, IStateData stateData)
    {
        base.OnCreate(machine, stateData);
        info = (PlayerBattleInfo)stateData;
    }

    public override void OnEnter()
    {
       
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {

    }

    public override void OnDispose()
    {
        base.OnDispose();
    }
}
