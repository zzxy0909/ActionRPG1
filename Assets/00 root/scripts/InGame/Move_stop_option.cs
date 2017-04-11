using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Move_stop_option
{
    public bool m_autoDash = false;
    public bool m_isPlaySkillorAttack = false; // attack 포함.
    public bool m_isPlayAvoid = false;
    public bool checkStopCase()
    {
        if (m_isPlaySkillorAttack == true)
            return true;
        if (m_isPlayAvoid == true)
            return true;

        return false;
    }
}
