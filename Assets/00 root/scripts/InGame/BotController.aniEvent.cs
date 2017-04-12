using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BotController : MonoBehaviour {

    public LegoEffect m_LegoEffect;

    public void ae_attack_start()
    {
    }

    public void ae_attack_end()
    {
        if (m_Bot_type == eBot_type.player)
        {
            // 플레이어가 계속 공격 버튼을 누르고 있으면 다음 공격 유지 기능 뺌. 약약강은 이전 공격이 연속 공격에 속하면 반복 해서 기본 공격(타임으로 연속 공격 속성을 관리 할수 있음.
            //Gui_PlayUI comp = GuiMgr.Instance.Find<Gui_PlayUI>();
            //if (comp != null && comp.m_isDownAttackSkill == true)
            //{
            //    int att_val = m_Animator.GetInteger(DefineID.Ani_attack);
            //    if(att_val >= DefineID.Num_StartLinkAttack)
            //    {
            //        att_val++; 
            //        // 연속 공격
            //        if (att_val >= (DefineID.Num_StartLinkAttack + DefineID.Max_LinkAttackCount) )
            //        {
            //            att_val = DefineID.Num_StartLinkAttack;
            //        }

            //    }
            //    m_Animator.SetInteger(DefineID.Ani_attack, att_val);
            //    m_Animator.SetTrigger(DefineID.Ani_attack_start);
            //    return;
            //}

            int att_val = m_Animator.GetInteger(DefineID.Ani_attack);
            if (att_val >= DefineID.Num_StartLinkAttack
                && att_val < (DefineID.Num_StartLinkAttack + DefineID.Max_LinkAttackCount))
            {
                // 연속 공격 대기
                m_Move_stop_option.m_isPlaySkillorAttack = false;
                return;
            }
        }
        m_Move_stop_option.m_isPlaySkillorAttack = false;
        m_Animator.SetInteger(DefineID.Ani_attack, 0);
    }

    public void ae_legoEffect(int v_id)
    {
        LegoEffect.SetEffect effect = m_LegoEffect.m_setEffectList[v_id];
        GameObject obj = Instantiate(effect.effectPrefab);

        if (effect.attach == true)
        {
            obj.transform.SetParent(effect.boneLink.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.localRotation = Quaternion.identity;
        }else
        {
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
        }
    }
}
