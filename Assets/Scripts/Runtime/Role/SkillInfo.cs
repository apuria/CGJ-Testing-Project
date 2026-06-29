using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]
public class SkillInfo : ScriptableObject
{
    public string skillName;
    public Sprite skillIcon;
    [TextArea(3, 10)]
    public string skillDescription;
    public int Damage;
    public bool isAOE;

    public bool hasBuff;
    public BuffInfo buff;
    //TODO:
    /*
    1. 攻击特效
    2. 受击特效
    */
}
