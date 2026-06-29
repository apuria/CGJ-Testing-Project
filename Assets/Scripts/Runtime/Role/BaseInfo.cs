using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInfo : ScriptableObject
{
//TODO: 基础信息
/*
1. 攻击特效 + 敌人受击特效
2. 立绘
3. 头像
4. 缩放比例
*/
    public string Name;
    public Stat maxHp;
    public Stat hp;
    public Stat maxMp;
    public Stat mp;
    public Stat attack;
    public Stat defense;
    public Stat speed;
    public List<SkillInfo> skills;
    public SkillInfo ultimateSkill;

    private Dictionary<BuffInfo, int> buffs;

    public void AddBuff(BuffInfo buff)
    {
        if (buffs.ContainsKey(buff))
        {
            buffs[buff] = buff.duration;
            return;
        }
        buffs.Add(buff, buff.duration);
        BuffChangeValue(buff.buffType, buff.value);
    }

    public void RemoveBuff(BuffInfo buff)
    {
        if (buffs == null)
            return;
        if(!buffs.ContainsKey(buff))
            return;
        buffs.Remove(buff);
        BuffChangeValue(buff.buffType, -buff.value);
    }

    private void BuffChangeValue(BuffType type, float value)
    {
        switch (type)
        {
            case BuffType.MAXHP:
                maxHp.value += value;
                break;
            case BuffType.HP:
                hp.value += value;
                if(hp.value > maxHp.value)
                    hp.value = maxHp.value;
                break;
            case BuffType.MAXMP:  
                maxMp.value += value;
                break;
            case BuffType.MP:   
                mp.value += value;
                if (mp.value > maxMp.value)
                    mp.value = maxMp.value;
                break;
            case BuffType.ATK:
                attack.value += value;
                break;
            case BuffType.DEF:
                defense.value += value;
                break;
            case BuffType.SPD:  
                speed.value += value;
                break;
        }
    }

}
