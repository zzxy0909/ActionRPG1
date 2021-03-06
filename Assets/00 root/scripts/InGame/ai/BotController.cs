﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum eBot_type
{
    player,
    enemy,
    boss,
}

public partial class BotController : MonoBehaviour {
    
    public Animator m_Animator;
    public Move_stop_option m_Move_stop_option = new Move_stop_option();

    public eBot_type m_Bot_type = eBot_type.enemy;  // BotType 에서 player, Enemy 등 으로 구분 할 수도 있음.
    public Transform m_posHPbar;
    public NavMeshAgent m_controllerAgent;

    
    // Use this for initialization
    void Start () {
        if(m_Animator == null)
        {
            m_Animator = GetComponent<Animator>();
        }
        if(m_LegoEffect == null)
        {
            m_LegoEffect = GetComponent<LegoEffect>();
        }
    }

    //========================== dash attack 관련
    float lastCheckTime = 0;
    float checkTime = 0.1f;
    int checkDashCount = 0;
    private void Update()
    {
        if(lastCheckTime < Time.time)
        {
            lastCheckTime = Time.time + checkTime;
            int runNum = m_Animator.GetInteger(DefineID.Ani_run);
            if (runNum <= 0)
            {
                resetDashFlag();
            }
            else if(runNum > 0 
                // && m_Move_stop_option.m_isJumping == false
                )
            {
                checkDashCount++;
                if( DefineID.Max_autoDashCheckValue < checkDashCount )
                {
                    m_Move_stop_option.m_autoDash = true;
                }
            }
        }
    }

    void resetDashFlag()
    {
        checkDashCount = 0;
        m_Move_stop_option.m_autoDash = false;
    }
    //================================================

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

    
    public void SetRun()
    {
        m_Animator.SetInteger(DefineID.Ani_run, 1);
    }
    public void SetRun_end()
    {
        resetDashFlag();
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
        if (m_Move_stop_option.m_isPlaySkillorAttack == true)
            return;

        int att_val = m_Animator.GetInteger(DefineID.Ani_attack);
        if (att_val >= DefineID.Num_StartLinkAttack
                && att_val < (DefineID.Num_StartLinkAttack + DefineID.Max_LinkAttackCount)
                && n == DefineID.Num_StartLinkAttack
                )
        {
            att_val++;
            // 연속 공격
            if (att_val == (DefineID.Num_StartLinkAttack + DefineID.Max_LinkAttackCount))
            {
                att_val = DefineID.Num_StartLinkAttack;
            }
            m_Move_stop_option.m_isPlaySkillorAttack = true;
            m_Animator.SetInteger(DefineID.Ani_attack, att_val);
            m_Animator.SetTrigger(DefineID.Ani_attack_start);
            return;
        }

        m_Move_stop_option.m_isPlaySkillorAttack = true;
        m_Animator.SetTrigger(DefineID.Ani_attack_start);
        m_Animator.SetInteger(DefineID.Ani_attack, n);
    }

    // ex 10~20 번 사이 공격은 대시 후 공격 처리.==> Ani_dash 후 Ani_attack n 되게 수정 필요.
    public void SetDashAttack(int n)
    {
        if (m_Move_stop_option.m_isPlaySkillorAttack == true)
            return;
        SetDashMove(true);

        SetAttack(n);

        //m_Move_stop_option.m_isPlaySkillorAttack = true;
        //m_Animator.SetTrigger(DefineID.Ani_attack_start);

        //m_Animator.SetInteger(DefineID.Ani_attack, n);
    }

    void SetDashMove(bool v_playAni)
    {
        if (v_playAni == true)
        {
            m_Animator.SetBool(DefineID.Ani_dash, true);
        }
        StartCoroutine(DashMove(v_playAni));
    }

    // 0.3초간 전방으로 이동
    float dashTime = 0.3f;
    float dathDistence = 3f;
    float dashSpeed = 200;
    bool b_dash = false;
    IEnumerator DashMove(bool v_playAni)
    {
        if (b_dash == true)
            yield break;
        b_dash = true;
        // 대시를 할때 런 상테는 종료 함.
        Player_controller.Instance.m_BotController.SetRun_end();

        if (v_playAni == true)
        {
            m_Animator.SetBool(DefineID.Ani_dash, true);
        }

        Vector3 resultPos = m_controllerAgent.transform.position + m_controllerAgent.transform.forward * dathDistence;
        m_controllerAgent.SetDestination(resultPos);
        float speed_old = m_controllerAgent.speed;
        float acceleration_old = m_controllerAgent.acceleration;
        m_controllerAgent.speed = dashSpeed;
        m_controllerAgent.acceleration = dashSpeed * 2;
        m_controllerAgent.Resume();
        yield return new WaitForSeconds(dashTime);
        m_controllerAgent.speed = speed_old;
        m_controllerAgent.acceleration = acceleration_old;
        m_controllerAgent.Stop();

        b_dash = false;
        m_Animator.SetBool(DefineID.Ani_dash, false);
    }
}
