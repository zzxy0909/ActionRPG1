using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AiEnemy_controller : MonoBehaviour {
    public enum Tendency { Peace, Aggressive, Defence }; // 성향 => Peace:피격시 대응, Aggressive:적을 찾아서 공격, Defence:접근하는 적을 공격
    public enum AiEnemyState { Moving = 0, Attack = 1, Escape = 2, Idle = 3, Follow = 4 };
    public AiEnemyState m_currentState = AiEnemyState.Idle;
    public AiEnemyState m_lastState = AiEnemyState.Idle;
    public Transform m_MasterTransform;
    public BotController m_BotController;
    public Transform m_AttackTarget;
    public float m_ApproachDistance = 3.0f;  //  접근후 attack
    public float m_DetectRange = 10; // 적군 인식 범위

    public FindAttackTarget_option m_FindAttackTarget_option;

    public NavMeshAgent m_NavMeshAgent;

    Transform m_thisTransform;

    // Use this for initialization
    void Start () {
        if(m_FindAttackTarget_option == null)
        {
            m_FindAttackTarget_option = GetComponent<FindAttackTarget_option>();
        }

        AiManager.Instance.Add_AiEnemy(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
