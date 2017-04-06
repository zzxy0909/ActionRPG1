using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gui 기반에서 생성되어야 하는 것들이 있어서 GuiBase 를 이용 한다.
public class Gui_BotUIManager : GuiBase
{
    public GameObject m_prefab_player_botUI;
    public GameObject m_prefab_enemy_botUI;
    public GameObject m_prefab_boss_botUI;

    Transform thisTrans;
    private void Awake()
    {
        thisTrans = this.transform;
    }
    public void Start()
    {
        Gui_BotUIManager tmp = GuiMgr.Instance.Find<Gui_BotUIManager>();
        if (tmp == null)
        {
            GuiMgr.Instance.AddShowEntity(this);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    public GameObject Add_BotUI(eBot_type v_type)
    {
        GameObject obj = null;
        switch(v_type)
        {
            case eBot_type.player:
                obj = Instantiate(m_prefab_player_botUI);
                obj.transform.SetParent(thisTrans);
                obj.transform.localScale = Vector3.one;
                break;
            case eBot_type.enemy:
                obj = Instantiate(m_prefab_enemy_botUI);
                obj.transform.SetParent(thisTrans);
                obj.transform.localScale = Vector3.one;
                break;
            case eBot_type.boss:
                obj = Instantiate(m_prefab_boss_botUI);
                obj.transform.SetParent(thisTrans);
                obj.transform.localScale = Vector3.one;
                break;
        }

        return obj;
    }	
}
