using UnityEngine;
using System.Collections;


public class Gui_Login : GuiBase
{
    string _btnLogin = "Panel/btnLogin";

    public override void SetInspactor_InitTransList()
    {
        _usedTransListKey.Clear(); _usedTransListObject.Clear();
        // key - object 쌍으로 설정.
		Set_InitTransListKey(_btnLogin, LoginClick);

        //_usedTransListKey.Add(_btnLogin); _usedTransListObject.Add(this.transform.FindChild(_btnLogin));
        //Get_usedTrans(_btnLogin).GetComponent<UIButton>().onClick.Clear();
        //EventDelegate.Add(Get_usedTrans(_btnLogin).GetComponent<UIButton>().onClick, LoginClick);
    }
    
    /// <summary>
    /// "로그인" 버튼 클릭시 호출...
    /// </summary>
    public void LoginClick()
    {
        Debug.Log("~~~~~~~~~~~ LoginClick !!!");


    }


}
