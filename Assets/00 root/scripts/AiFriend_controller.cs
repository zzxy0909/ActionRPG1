using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiFriend_controller : MonoBehaviour {
    public enum AIStatef { Moving = 0, Attack = 1, Escape = 2, Idle = 3, FollowMaster = 4 };
    public AIStatef m_followState = AIStatef.Idle;
    public Transform m_MasterTransform;
    public BotController m_BotController;
    public GameObject m_AttackTarget;
    public float m_ApproachDistance = 3.0f;
    public float m_DetectRange = 15;

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

        m_followState = AIStatef.FollowMaster;
    }
	// Use this for initialization
	void Start () {
        StartCoroutine(Startup());

	}

    float lastTime_Attack = 0;
    void CheckStatePlay()
    {
        switch(m_followState)
        {
            case AIStatef.FollowMaster:
                if( (m_MasterTransform.position - transform.position).magnitude <= m_ApproachDistance)
                {
                    m_followState = AIStatef.Idle;
                }else
                {

                }
                break;
            case AIStatef.Idle:

                break;
        }
    }

    float checkTime = 0.5f;
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
