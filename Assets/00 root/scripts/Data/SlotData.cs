using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlotData
{
    public int m_slot_index;
    public string m_equipID;
    public GameObject m_ObjCurrent;
    public Transform m_LinkObject;
    public Vector3 m_joinPositionOffset;
    public Vector3 m_joinScaleOffset;
}