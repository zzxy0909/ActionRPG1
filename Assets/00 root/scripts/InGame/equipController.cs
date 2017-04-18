using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class equipController : MonoBehaviour {
    public Transform m_linkBone;
    public SlotData m_CurrentData = null;

	// Use this for initialization
	void Start () {
        Set_DefaultEquip();
    }

    void Set_DefaultEquip()
    {
        StartCoroutine(IE_Set_DefaultEquip());
    }
    IEnumerator IE_Set_DefaultEquip()
    { 
        while(Player_controller.Instance.m_StartupOK == false)
        {
            yield return new WaitForEndOfFrame();
        }

        // m_ObjEquip = EquipManager.Instance.SpawnEquip("sword001", m_linkBone);
        EquipManager.Instance.ChangeEquip(0);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
