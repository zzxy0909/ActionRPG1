using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiFriend_controller : MonoBehaviour {
    public enum AIStatef { Moving = 0, Attack = 1, Escape = 2, Idle = 3, FollowMaster = 4 };
    public AIStatef m_currentState = AIStatef.Idle;
    public AIStatef m_lastState = AIStatef.Idle;
    public Transform m_MasterTransform;
    public BotController m_BotController;
    public Transform m_AttackTarget;
    public float m_ApproachDistance = 3.0f;
    public float m_DetectRange = 15;

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

        SetState(AIStatef.FollowMaster);
    }
	// Use this for initialization
	void Start () {
        StartCoroutine(Startup());
	}
    void SetState(AIStatef st)
    {
        m_currentState = st;
    }
    void SetLastState(AIStatef st)
    {
        m_lastState = st;
    }

    void FollowMaster()
    {
        m_NavMeshAgent.SetDestination(m_MasterTransform.position);
        m_NavMeshAgent.Resume();
        m_BotController.SetRun();

        SetLastState(AIStatef.FollowMaster);
    }
    void Play_idle()
    {
        if (isInApproachDistance())
        {
            m_NavMeshAgent.Stop();
            m_BotController.SetRun_end();
            m_BotController.SetIdle();
            SetLastState(AIStatef.Idle);
        }else if(isInDetectRange() )
        {
            // attack skill Range 를 검사 하여 해당 거리에 공격 시도 후 다시 idle

            // idle
            m_BotController.SetRun_end();
            m_BotController.SetIdle();
            SetLastState(AIStatef.Idle);
        }
        else // ApproachDistance 밖에서 적을 찾지 못 했으면 FollowMaster
        {
            SetState(AIStatef.FollowMaster);
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
            case AIStatef.FollowMaster:
                if(isInApproachDistance() )
                {
                    SetState(AIStatef.Idle);
                    Play_idle();
                }
                else 
                {
                    FollowMaster();
                }
                break;
            case AIStatef.Idle:
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
