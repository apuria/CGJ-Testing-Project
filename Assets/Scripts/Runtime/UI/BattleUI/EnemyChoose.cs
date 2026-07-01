using System.Collections.Generic;
using UnityEngine;

public class EnemyChoose : MonoBehaviour
{
    public List<EnemyChooseSlot> enemyChooseSlots;
    public void UpdateUI(List<EnemyInfo> enemies)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (i >= enemyChooseSlots.Count)
                break;
            enemyChooseSlots[i].UpdateUI(enemies[i].avatar);
        }
    }
}
