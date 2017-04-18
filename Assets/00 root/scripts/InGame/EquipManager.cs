using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour {
    #region Singleton
    private static EquipManager _instance = null;
    public static EquipManager Instance
    {
        get
        {
            return _instance;
        }
    }
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this; // GameObject.FindObjectOfType<EquipManager>();
        }
        else
        {
            GameObject.Destroy(this.gameObject);
            return;
        }

        Init_dicEquipData();
    }
    #endregion Singleton

    public EquipData[] m_arrEquipData;
    // m_arrEquipData 량이 많을 것 같아 Dictionary 준비.
    Dictionary<string, EquipData> m_dicEquipData = new Dictionary<string, EquipData>();
    public EquipData Get_EquipData(string v_expl)
    {
        if (m_dicEquipData.ContainsKey(v_expl))
        {
            return m_dicEquipData[v_expl];
        }else
        {
            return null;
        }
    }
    void Init_dicEquipData()
    {
        m_dicEquipData.Clear();
        for (int i =0; i< m_arrEquipData.Length; i++)
        {
            if(m_arrEquipData != null)
            {
                m_dicEquipData.Add(m_arrEquipData[i].m_equipPrefab.name, m_arrEquipData[i]);
            }
            
        }
    }
    //==============================================================

    // Use this for initialization
    void Start () {
        

    }

    public GameObject SpawnEquip(string v_equip_name, Transform v_joinObj)
    {
        EquipData equip_data = Get_EquipData(v_equip_name);
        if (equip_data == null && v_joinObj == null)
        {
            Debug.Log(" SpawnEquip() ==> equip_data == null && v_joinObj == null ");
            return null;
        }
        else
        {
            GameObject obj = Instantiate(equip_data.m_equipPrefab);
            obj.transform.SetParent(v_joinObj);
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            return obj;
        }
    }
    public void ChangeEquip(int v_slot_index)
    {
        equipController equip = Player_controller.Instance.m_equipController;

        // old 복귀
        if(equip.m_CurrentData != null && equip.m_CurrentData.m_slot_index >= 0)
        {
            SlotData slot_odl = equip.m_CurrentData; // SlotManager.Instance.m_arrSlotData[equip.m_select_index];
            // slot_odl.m_LinkObject.gameObject.SetActive(true);
            slot_odl.m_ObjCurrent.transform.SetParent(slot_odl.m_LinkObject);
            slot_odl.m_ObjCurrent.transform.localScale = Vector3.one + slot_odl.m_joinScaleOffset;
            slot_odl.m_ObjCurrent.transform.localPosition = Vector3.zero + slot_odl.m_joinPositionOffset;
            slot_odl.m_ObjCurrent.transform.localRotation = Quaternion.identity;
        }

        // SlotData 정보 전달후 parent 설정.
        SlotData slot_data = SlotManager.Instance.m_arrSlotData[v_slot_index];
        equip.m_CurrentData = slot_data;
        equip.m_CurrentData.m_ObjCurrent.transform.SetParent(equip.m_linkBone);
        equip.m_CurrentData.m_ObjCurrent.transform.localScale = Vector3.one;
        equip.m_CurrentData.m_ObjCurrent.transform.localPosition = Vector3.zero;
        equip.m_CurrentData.m_ObjCurrent.transform.localRotation = Quaternion.identity;

        //slot_data.m_ObjCurrent.transform.SetParent(slot_data.m_LinkObject);
        //slot_data.m_ObjCurrent.transform.localScale = Vector3.one + slot_data.m_joinScaleOffset;
        //slot_data.m_ObjCurrent.transform.localPosition = Vector3.zero + slot_data.m_joinPositionOffset;
        //slot_data.m_ObjCurrent.transform.localRotation = Quaternion.identity;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
