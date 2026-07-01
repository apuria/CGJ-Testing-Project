using System.Collections;
using System.Collections.Generic;
using UniFramework.Event;
using UnityEngine;
using UnityEngine.UI;

public class BattlePanel : BasePanel
{
//TODO:
/*
1. 
*/

#region 属性

    public Button btnSetting;
    public Button btnState;
    public Button btnRetry;
    public Button btnReturn;
    public Button btnQuit;
    public Button btnAttack;
    public Button btnDefend;
    public Button btnSkill;
    public Button btnRun;

    public HPandMP hpadnmp;
    public ActionList actionList;
    public Enemies enemies;
    public Roles roles;
    public Skills skills;
    public RoleState roleState;
    public EnemyChoose enemyChoose;

#endregion

#region 生命周期

    protected void Awake()
    {
        eventGroup = new();
    }

    protected void Start()
    {
        
    }

    protected void OnDestroy()
    {
        eventGroup.RemoveAllListener();
    }
#endregion

#region 逻辑控制

    public override void HideMe()
    {

    }

    public override void ShowMe()
    {

    }

#endregion

#region 事件监听

    public void OnHandleEventMessage(IEventMessage message)
    {
        
    }

#endregion  
}
