using System.Collections;
using System.Collections.Generic;
using UniFramework.Machine;
using UnityEngine;

public class DiceBattleState : BaseBattleState
{

    public DiceBattleInfo info;
    public override void OnCreate(StateMachine machine, IStateData stateData)
    {
        base.OnCreate(machine, stateData);
        info = (DiceBattleInfo)stateData;
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
}
