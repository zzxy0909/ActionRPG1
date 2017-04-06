using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BotUI : MonoBehaviour {
    public UISlider m_HPbar;
    Transform m_posHPbar; // 설정후 start 에서 UiFollowTarget 정함.

    // hpbar 추가.
    public void Set_UI_Info(Transform v_posHPbar)
    {
        m_posHPbar = v_posHPbar;
        

    }
    private void Start()
    {
        UiFollowTarget follow = m_HPbar.GetComponent<UiFollowTarget>();
        follow.Set_Target(m_posHPbar);
    }
    // Update is called once per frame
    void Update () {
		
	}
}
