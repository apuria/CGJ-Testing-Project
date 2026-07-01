using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{
    public List<SkillSlot> skillSlots;
    public void UpdateUI(List<SkillInfo> skills)
    {
        for (int i = 0; i < skills.Count; i++)
        {
            if (i >= skillSlots.Count)
                break;
            skillSlots[i].UpdateUI(skills[i].skillIcon, skills[i].skillName);
        }
    }
}
