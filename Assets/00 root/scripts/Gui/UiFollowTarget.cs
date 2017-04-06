using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiFollowTarget : MonoBehaviour {
    public Transform m_target;

    public Camera m_gameCamera;
    public Camera m_uiCamera;

    public float m_yOffset = 0f; // UI 포지션 기준 + 옵션값
    Transform m_thisTrans;
    private void Awake()
    {
        m_thisTrans = this.transform;
    }
    private void OnEnable()
    {
        // 혹시 꺼쪗다 켜져서 올드 좌표에 남아 있었다면, 시작은 무조껀 극좌표에서 시작
        m_thisTrans.position = new Vector3(10000.0f, 10000.0f, 10000.0f);
    }

    public void Set_Target(Transform v_target)
    {
        m_target = v_target;
    }
    // Update is called once per frame
    void Update () {
        if (m_target == null )
        {
            // UIBot 에서 오브젝트 관리. GameObject.Destroy(gameObject);
            return;
        }
        if( m_target.gameObject.activeSelf == false)
        {
            return;
        }
        if(m_gameCamera == null)
        {
            m_gameCamera = Camera.main;
        }
        if(m_uiCamera == null)
        {
            m_uiCamera = GuiMgr.Instance.GetBackCamera(); // 팝업창 뒤로 그려지도록.
        }
        Vector3 posTmp = m_gameCamera.WorldToViewportPoint(m_target.position);
        bool l_isVisible = (posTmp.x > 0f && posTmp.x < 1f && posTmp.y > 0f && posTmp.y < 1f); // 뷰포트에 있는가?

        if (l_isVisible)
        {
            
            transform.position = m_uiCamera.ViewportToWorldPoint(posTmp);
            posTmp = transform.localPosition;
            posTmp.x = 0f; // Mathf.FloorToInt(posTmp.x);
            posTmp.y += m_yOffset; // Mathf.FloorToInt(posTmp.y) + m_yOffset;
            posTmp.z = 0f;
            transform.localPosition = posTmp;
        }
    }
}
