using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public List<EnemySlot> enemySlots;
    public void UpdateUI(List<EnemyInfo> enemies)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (i >= enemySlots.Count)
                break;
            enemySlots[i].UpdateUI(enemies[i].avatar, enemies[i].scale);
        }
    }
}
