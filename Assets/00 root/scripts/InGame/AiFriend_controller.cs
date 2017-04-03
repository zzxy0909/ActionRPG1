using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiFriend_controller : MonoBehaviour {
    public enum AiFriendState { Moving = 0, Attack = 1, Escape = 2, Idle = 3, Follow = 4 };
    public AiFriendState m_currentState = AiFriendState.Idle;
    public AiFriendState m_lastState = AiFriendState.Idle;
    public Transform m_MasterTransform;
    public BotController m_BotController;
    public Transform m_AttackTarget;
    public float m_ApproachDistance = 3.0f;  // 마스터 접근후 idle
    public float m_DetectRange = 15; // 적군 인식 범위

    public NavMeshAgent m_NavMeshAgent;

    Transform m_thisTransform;

    IEnumerator Startup()
    {
        m_thisTransform = this.transform;
        if (m_MasterTransform == null)
        {
            while(Player_controller.Instance == null)
            {
                yield return null;
            }

            m_MasterTransform = Player_controller.Instance.transform;
        }
        if(m_BotController == null)
        {
            m_BotController = m_thisTransform.GetComponentInChildren<BotController>();
        }
        if(m_NavMeshAgent == null)
        {
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
        }

        SetState(AiFriendState.Follow);
    }
	// Use this for initialization
	void Start () {
        StartCoroutine(Startup());
	}
    void SetState(AiFriendState st)
    {
        m_currentState = st;
    }
    void SetLastState(AiFriendState st)
    {
        m_lastState = st;
    }

    void FollowMaster()
    {
        m_NavMeshAgent.SetDestination(m_MasterTransform.position);
        m_NavMeshAgent.Resume();
        m_BotController.SetRun();

        SetLastState(AiFriendState.Follow);
    }
    void Stop_Move()
    {
        m_NavMeshAgent.Stop();
        if (m_lastState != AiFriendState.Idle)
        {
            m_BotController.SetRun_end();
            m_BotController.SetIdle();
            SetLastState(AiFriendState.Idle);
        }
    }

    void PlayAttack()
    {
        if (isInDetectRange())
        {

        }
    }

    public void FindAttackTarget_option()
    {
        m_checkEnemy = AiManager.Instance.Get_NearAiEnemy(m_thisTransform.position);
        if (m_checkEnemy != null)
            m_AttackTarget = m_checkEnemy.transform;
    }

    public AiEnemy_controller m_checkEnemy;
    void Play_idle()
    {
        if (isInApproachDistance())
        {
            Stop_Move();
        }
        else if(isInDetectRange() )
        {
            // attack skill Range 를 검사 하여 해당 거리에 공격 시도 후 다시 idle

            // idle
            Stop_Move();
        }
        else // ApproachDistance 밖에서 적을 찾지 못 했으면 FollowMaster
        {
            FindAttackTarget_option();
            SetState(AiFriendState.Follow);
        }
    }
    bool isInApproachDistance()
    {
        return (m_MasterTransform.position - m_thisTransform.position).magnitude <= m_ApproachDistance ;
    }
    bool isInDetectRange()
    {
        if(m_AttackTarget == null)
            return false;

        return (m_AttackTarget.position - m_thisTransform.position).magnitude <= m_DetectRange;
    }

    float lastTime_Attack = 0;
    void CheckStatePlay()
    {
        switch(m_currentState)
        {
            case AiFriendState.Follow:
                if(isInApproachDistance() )
                {
                    SetState(AiFriendState.Idle);
                    Play_idle();
                }
                else 
                {
                    FollowMaster();
                }
                break;
            case AiFriendState.Idle:
                Play_idle();
                break;
        }
    }

    float checkTime = 0.3f;
    float updateTime = 0;
	// Update is called once per frame
	void Update () {
        // 설정된 마스터를 기준으로 행동 함.
        if (m_MasterTransform == null)
            return;

        if (updateTime <= checkTime)
        {
            updateTime += Time.deltaTime;
        }else
        {
            updateTime = 0;
            CheckStatePlay();
        }
	}
}
