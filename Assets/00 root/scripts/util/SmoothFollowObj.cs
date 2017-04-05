using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollowObj : MonoBehaviour {
    public Transform m_Target;
    public Vector3 m_offset;
    public float m_speedTime = 0.1f;

    Transform thisTrans;
    void Awake()
    {
        thisTrans = this.transform;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (m_Target == null)
            return;
        Vector3 targetPos = m_Target.position + m_offset;
        
        if( (targetPos - thisTrans.position).magnitude < 0.05f )
        {
            return;
        }

        thisTrans.position = Vector3.Lerp(thisTrans.position, targetPos , m_speedTime);


    }
}
