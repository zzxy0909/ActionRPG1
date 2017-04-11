using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillInfo
{
    public int m_id;// in table.
    public float m_coolTime = 0;
    public int m_attackAniNum;
    public float m_attackRange = 0;
    public bool m_canCalcel = false;
    public int m_targetType = 0; // 0 : attack enemy, 1: our team assistance
    public float m_lastUseTime = 0;

    public float m_hpLimit = 0;
    public int m_useChance = 0;

    public enum eSkillType
    {
        NomalAttack,
        Skill
    }
    public eSkillType m_SkillType = eSkillType.NomalAttack;


}
