using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gui_PlayUI : GuiBase
{
    public int m_lastAttackSkill = 0;
    public bool m_isDownAttackSkill = false;

    // float m_minTime_attackSkill = 1f; // 다른 공격 시작 은 최소 1초.
    public void UpAttackSkill()
    {
        m_isDownAttackSkill = false;

        // 더블 클릭 및 연타시 최소 타임에 해당 하는 공격 동작은 유지 한다.
        //if(Player_controller.Instance.m_BotController.m_Move_stop_option.m_isPlaySkillorAttack ==  false)
        //Player_controller.Instance.m_BotController.m_Move_stop_option.m_isPlaySkillorAttack = false;
    }

    public void DownAttack_a()
    {
        // bot table default attack no 사용 
        int n = 2; // test
        // 계속 같은 공격 버튼을 누르고 있으면 공격 유지 및 더블 클릭 시 한번만 공격 진행 하기.
        if(m_lastAttackSkill == n
            && Player_controller.Instance.m_BotController.m_Move_stop_option.m_isPlaySkillorAttack == true
            )
        {
            return;
        }
        DownAttackSkill(n);
    }
    public void DownAttack_b()
    {
        DownAttackSkill(2);
    }
    public void DownAttack_c()
    {
        DownAttackSkill(3);
    }
    public void DownAttack_d()
    {
        DownAttackSkill(4);
    }
    public void DownAttack_e()
    {
        DownAttackSkill(5);
    }
    public void DownAttackSkill(int n)
    {
        m_lastAttackSkill = n;
        m_isDownAttackSkill = true;
        Player_controller.Instance.m_BotController.SetAttack(n);
    }
}
