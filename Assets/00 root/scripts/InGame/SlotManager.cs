using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour {
    #region Singleton
    private static SlotManager _instance = null;
    public static SlotManager Instance
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
            _instance = this; // GameObject.FindObjectOfType<SlotManager>();
        }
        else
        {
            GameObject.Destroy(this.gameObject);
            return;
        }
    }
    #endregion Singleton

    public SlotData[] m_arrSlotData;
    
    // Use this for initialization
    void Start () {
        TestSpawnData();

    }
    void TestSpawnData()
    {
        for(int i=0; i< m_arrSlotData.Length; i++)
        {
            SpawnSlotData(i);
        }
    }

    public GameObject SpawnSlotData(int v_index)
    {
        SlotData l_data = m_arrSlotData[v_index];
        if (l_data == null)
        {
            Debug.Log(" SpawnSlotData() ==> l_data == null ");
            return null;
        }
        else
        {
            l_data.m_slot_index = v_index;
            l_data.m_ObjCurrent = EquipManager.Instance.SpawnEquip(l_data.m_equipID, l_data.m_LinkObject);
            return l_data.m_ObjCurrent;
        }
    }
    
	
}
