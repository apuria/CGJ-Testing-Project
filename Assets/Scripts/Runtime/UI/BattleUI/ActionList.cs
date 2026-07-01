using System.Collections.Generic;
using UnityEngine;

public class ActionList : MonoBehaviour
{
    public List<ActionSlot> actions;

    public void UpateUI(List<BaseInfo> actionList)
    {
        for (int i = 0; i < actions.Count; i++)
        {
            actions[i].UpdateUI(actionList[i].avatar);
        }
    }
}
