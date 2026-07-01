using System.Collections.Generic;
using UnityEngine;

public class Roles : MonoBehaviour
{
    public List<RoleSlot> roleSlots;
    public void UpdateUI(List<RoleInfo> roles)
    {
        for (int i = 0; i < roles.Count; i++)
        {
            if (i >= roleSlots.Count)
                break;
            roleSlots[i].UpdateUI(roles[i].avatar, roles[i].scale);
        }
    }
}
