using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour {

    public Animator m_Animator;

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
        if( m_Animator.GetInteger(DefString.Ani_run) == 0 
           && m_Animator.GetInteger(DefString.Ani_avoid) == 0
                )
        {
            m_Animator.SetTrigger(DefString.Ani_idle);
        }
        
    }
    public void SetRun()
    {
        m_Animator.SetInteger(DefString.Ani_run, 1);
    }
    public void SetRun_end()
    {
        m_Animator.SetInteger(DefString.Ani_run, 0);
    }
    public void SetAvoid()
    {
        m_Animator.SetInteger(DefString.Ani_avoid, 1);
    }
    public void SetAvoid_end()
    {
        m_Animator.SetInteger(DefString.Ani_avoid, 0);
    }
}
