using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillButtonData
{
    public string expl_name;
    public int attack_num;    
}

public class Gui_PlayUI : GuiBase
{
    public int m_lastAttackSkill = 0;
    public bool m_isDownAttackSkill = false;
    public SkillButtonData[] m_arrSkillButtonData;
    public void Start()
    {
        // editor 진입시 오류 체크
        if (GuiMgr.Instance == null)
            return;

        Gui_PlayUI tmp = GuiMgr.Instance.Find<Gui_PlayUI>();
        if(tmp == null)
        {
            GuiMgr.Instance.AddShowEntity(this);
        }else
        {
            Destroy(this.gameObject);
            return;
        }
        testSkillButtonData();
    }

    void testSkillButtonData()
    {
        m_arrSkillButtonData = new SkillButtonData[6];
        for(int i=0; i< m_arrSkillButtonData.Length; i++ )
        {
            m_arrSkillButtonData[i] = new SkillButtonData();
        }
        
        m_arrSkillButtonData[0].expl_name = ""; // link attack
        m_arrSkillButtonData[0].attack_num = 90;
        m_arrSkillButtonData[1].expl_name = "expl001";
        m_arrSkillButtonData[1].attack_num = 2;
        m_arrSkillButtonData[2].expl_name = "expl002";
        m_arrSkillButtonData[2].attack_num = 2;
        m_arrSkillButtonData[3].expl_name = "";
        m_arrSkillButtonData[3].attack_num = 2;
        m_arrSkillButtonData[4].expl_name = "";
        m_arrSkillButtonData[4].attack_num = 2;
        m_arrSkillButtonData[5].expl_name = "";
        m_arrSkillButtonData[5].attack_num = 2;

    }

    // float m_minTime_attackSkill = 1f; // 다른 공격 시작 은 최소 1초.
    public void UpAttackSkill()
    {
        m_isDownAttackSkill = false;

        // 더블 클릭 및 연타시 최소 타임에 해당 하는 공격 동작은 유지 한다.
        //if(Player_controller.Instance.m_BotController.m_Move_stop_option.m_isPlaySkillorAttack ==  false)
        //Player_controller.Instance.m_BotController.m_Move_stop_option.m_isPlaySkillorAttack = false;
    }

    public void DownAttack_linkAttack()
    {
        // bot table default attack no 사용 
        int n = 90; // test
        Player_controller.Instance.m_BotController.Set_ExplosionData("");

        // 계속 같은 공격 버튼을 누르고 있으면 공격 유지 및 더블 클릭 시 한번만 공격 진행 하기.
        if (Player_controller.Instance.m_BotController.m_Move_stop_option.m_isPlaySkillorAttack == true
            )
        {
            return;
        }
        DownAttackSkill(n);
    }
    public void DownAttack_a()
    {
        if (Player_controller.Instance.m_BotController.m_Move_stop_option.m_isPlaySkillorAttack == true)
            return;

        // 장비 바꾸어 잡고 스킬 실행
        // front slot 부터 right ...  0~3 index
        EquipManager.Instance.ChangeEquip(0);

        SkillButtonData data = m_arrSkillButtonData[1];
        Player_controller.Instance.m_BotController.Set_ExplosionData(data.expl_name);
        DownAttackSkill(data.attack_num);
    }
    public void DownAttack_b()
    {
        if (Player_controller.Instance.m_BotController.m_Move_stop_option.m_isPlaySkillorAttack == true)
            return;

        EquipManager.Instance.ChangeEquip(1);

        SkillButtonData data = m_arrSkillButtonData[2];
        Player_controller.Instance.m_BotController.Set_ExplosionData(data.expl_name);
        DownAttackSkill(data.attack_num);
    }
    public void DownAttack_c()
    {
        if (Player_controller.Instance.m_BotController.m_Move_stop_option.m_isPlaySkillorAttack == true)
            return;

        EquipManager.Instance.ChangeEquip(2);

        SkillButtonData data = m_arrSkillButtonData[3];
        Player_controller.Instance.m_BotController.Set_ExplosionData(data.expl_name);
        DownAttackSkill(data.attack_num);
    }
    public void DownAttack_d()
    {
        if (Player_controller.Instance.m_BotController.m_Move_stop_option.m_isPlaySkillorAttack == true)
            return;

        EquipManager.Instance.ChangeEquip(3);

        SkillButtonData data = m_arrSkillButtonData[4];

        Player_controller.Instance.m_BotController.Set_ExplosionData(data.expl_name);
        DownAttackSkill(data.attack_num);
    }
    public void DownAttack_e()
    {
        if (Player_controller.Instance.m_BotController.m_Move_stop_option.m_isPlaySkillorAttack == true)
            return;
//        EquipManager.Instance.ChangeEquip(4);

        SkillButtonData data = m_arrSkillButtonData[5];
        Player_controller.Instance.m_BotController.Set_ExplosionData(data.expl_name);
        DownAttackSkill(data.attack_num);
    }

    public void DownAttackSkill(int v_attack_num)
    {
        m_lastAttackSkill = v_attack_num;

        if (Player_controller.Instance.m_BotController.m_Move_stop_option.m_autoDash == false)
        {
            Player_controller.Instance.m_BotController.SetAttack(v_attack_num);
        }
        else
        {
            Player_controller.Instance.m_BotController.SetDashAttack(v_attack_num);
        }
        // 

    }
}
