using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eBot_type
{
    player,
    enemy,
    boss,
}

public class BotController : MonoBehaviour {
    
    public Animator m_Animator;
    public Move_stop_option m_Move_stop_option = new Move_stop_option();

    public eBot_type m_Bot_type = eBot_type.enemy;  // BotType 에서 player, Enemy 등 으로 구분 할 수도 있음.
    public UI_BotUI m_BotUI;
    public Transform m_posHPbar;

    Gui_BotUIManager m_Gui_BotUIManager;
    public IEnumerator SetPlayer()
    {
        m_Bot_type = eBot_type.player;

        while (m_Gui_BotUIManager == null)
        {
            m_Gui_BotUIManager = GuiMgr.Instance.Find<Gui_BotUIManager>();
            yield return new WaitForEndOfFrame();
        }

        // BotUI 설정.
        GameObject guitmp = m_Gui_BotUIManager.Add_BotUI(eBot_type.player);
        if(guitmp != null)
        {
            m_BotUI = guitmp.GetComponent<UI_BotUI>();
            m_BotUI.Set_UI_Info(m_posHPbar);
        }

    }
    // Use this for initialization
    void Start () {
        if(m_Animator == null)
        {
            m_Animator = GetComponent<Animator>();
        }
        

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetIdle()
    {
        // 각 속성이 0 이면 idle
        if( m_Animator.GetInteger(DefineID.Ani_run) == 0 
           && m_Animator.GetInteger(DefineID.Ani_avoid) == 0
                )
        {
            m_Animator.SetTrigger(DefineID.Ani_idle);
           
        }
        
    }
    public void ae_attack_start()
    {
    }
    public void ae_attack_end()
    {
        if(m_Bot_type == eBot_type.player)
        {
            // 플레이어가 계속 공격 버튼을 누르고 있으면 다음 공격 유지.
            Gui_PlayUI comp = GuiMgr.Instance.Find<Gui_PlayUI>();
            if (comp != null && comp.m_isDownAttackSkill == true)
            {
                m_Animator.SetTrigger(DefineID.Ani_attack_start);
                return;
            }
        }
        m_Move_stop_option.m_isPlaySkillorAttack = false;
        m_Animator.SetInteger(DefineID.Ani_attack, 0);
    }

    public void SetRun()
    {
        m_Animator.SetInteger(DefineID.Ani_run, 1);
    }
    public void SetRun_end()
    {
        m_Animator.SetInteger(DefineID.Ani_run, 0);
    }
    public void SetAvoid()
    {
        m_Animator.SetInteger(DefineID.Ani_avoid, 1);
    }
    public void SetAvoid_end()
    {
        m_Animator.SetInteger(DefineID.Ani_avoid, 0);
    }
    public void SetAttack(int n)
    {
        m_Move_stop_option.m_isPlaySkillorAttack = true;
        m_Animator.SetTrigger(DefineID.Ani_attack_start);
        m_Animator.SetInteger(DefineID.Ani_attack, n);
    }
}
