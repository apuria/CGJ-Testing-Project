using System.Collections.Generic;
using UnityEngine;

public class RoleState : MonoBehaviour
{
    public List<StateSlot> stateSlots;
    public void UpdateUI(List<RoleInfo> roles)
    {
        for (int i = 1; i < roles.Count; i++)
        {
            if (i >= stateSlots.Count)
                break;
            stateSlots[i-1].UpdateUI(roles[i].avatar, roles[i].name, roles[i].hp.value * 1.0f / roles[i].maxHp.value, roles[i].mp.value * 1.0f / roles[i].maxMp.value);
        }
    }
}
