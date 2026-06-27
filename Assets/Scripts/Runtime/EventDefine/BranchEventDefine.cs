using System.Collections;
using System.Collections.Generic;
using UniFramework.Event;
using UnityEngine;

public class BranchEventDefine
{
//TODO:
/*
1. 按键触发分支选项
*/
    public class ChooseBranch : IEventMessage
    {
        public int branchId;
        public static void SendEventMessage(int branchId)
        {
            var msg = new ChooseBranch();
            msg.branchId = branchId;
            UniEvent.SendMessage(msg);
        }
    }
}
