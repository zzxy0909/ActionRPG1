using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BotUI : MonoBehaviour {
    public GameObject m_prefab_HPbar;
    public UISlider m_HPbar;
    // Transform m_posHPbar; // 설정후 start 에서 UiFollowTarget 정함.

    // hpbar 추가.
    public void Init_UI_Info(Transform v_posHPbar)
    {
        // m_posHPbar = v_posHPbar;

        Gui_BotUIRoot tmp_root = GuiMgr.Instance.Find<Gui_BotUIRoot>();
        if(tmp_root == null)
        {
            Debug.Log(" Gui_BotUIRoot null");
            return;
        }
        GameObject obj = Instantiate(m_prefab_HPbar);
        obj.transform.SetParent(tmp_root.transform);
        obj.transform.localScale = Vector3.one;

        m_HPbar = obj.GetComponent<UISlider>();

        UiFollowTarget follow = m_HPbar.GetComponent<UiFollowTarget>();
        follow.Set_Target(v_posHPbar);
    }
    private void Start()
    {
        
    }
    // Update is called once per frame
    void Update () {
		
	}
}
