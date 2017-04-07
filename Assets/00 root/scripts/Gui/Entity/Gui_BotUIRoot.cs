using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gui 기반에서 생성되어야 하는 것들이 있어서 GuiBase 를 이용 한다.
public class Gui_BotUIRoot : GuiBase
{
    Transform thisTrans;
    private void Awake()
    {
        thisTrans = this.transform;
    }
    public void Start()
    {
        // editor 진입시 오류 체크
        if (GuiMgr.Instance == null)
            return;

        Gui_BotUIRoot tmp = GuiMgr.Instance.Find<Gui_BotUIRoot>();
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

}
