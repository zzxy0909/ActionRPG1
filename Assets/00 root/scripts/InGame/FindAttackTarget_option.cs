using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindAttackTarget_option : MonoBehaviour {
    
    public bool m_isAggroTarget = false; // 어그로 로 인한 공격 중인가?
    public bool m_isAwhileAttack = false; // 잠시 공격 중인가?
    public bool canFindTarget()
    {
        if (m_isAggroTarget == true)
            return true;
        if (m_isAwhileAttack == true)
            return true;

        return false;
    }
}
