using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExplosionData
{
    public GameObject effectPrefab;
    public enum eTargetType
    {
        self,
        enemy,
    }
    public eTargetType m_TargetType = eTargetType.self;
    public Vector3 m_TargetPositionOffset;
}