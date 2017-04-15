using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour {
    #region Singleton
    private static ExplosionManager _instance = null;
    public static ExplosionManager Instance
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
            _instance = this; // GameObject.FindObjectOfType<ExplosionManager>();
        }
        else
        {
            GameObject.Destroy(this.gameObject);
            return;
        }

    }
    #endregion Singleton

    public ExplosionData[] m_arrExplosionData;
    // m_arrExplosionData 량이 많을 것 같아 Dictionary 준비.
    Dictionary<string, ExplosionData> m_dicExplosionData = new Dictionary<string, ExplosionData>();
    public ExplosionData Get_ExplosionData(string v_expl)
    {
        if (m_dicExplosionData.ContainsKey(v_expl))
        {
            return m_dicExplosionData[v_expl];
        }else
        {
            return null;
        }
    }
    void Init_dicExplosionData()
    {
        m_dicExplosionData.Clear();
        for (int i =0; i< m_arrExplosionData.Length; i++)
        {
            if(m_arrExplosionData != null)
            {
                m_dicExplosionData.Add(m_arrExplosionData[i].effectPrefab.name, m_arrExplosionData[i]);
            }
            
        }
    }
    //==============================================================

    // Use this for initialization
    void Start () {
        Init_dicExplosionData();

    }

    public void SpawnExplosion(string v_expl_name, GameObject v_attacker, GameObject v_target)
    {
        ExplosionData expl_data = Get_ExplosionData(v_expl_name);
        if(expl_data != null)
        {
            Quaternion spawn_rot = v_attacker.transform.rotation;
            Vector3 spawn_pos = Vector3.zero;

            ExplosionData.eTargetType l_type = expl_data.m_TargetType;
            switch(l_type)
            {
                case ExplosionData.eTargetType.self:
                    spawn_pos = v_attacker.transform.position + (v_attacker.transform.forward * expl_data.m_TargetPositionOffset.z) + new Vector3(0, expl_data.m_TargetPositionOffset.y, 0) ;
                    break;
                case ExplosionData.eTargetType.enemy:
                    if(v_target != null)
                    {
                        spawn_pos = v_target.transform.position + expl_data.m_TargetPositionOffset;
                    }else
                    {
                        spawn_pos = v_attacker.transform.position + (v_attacker.transform.forward * expl_data.m_TargetPositionOffset.z) + new Vector3(0, expl_data.m_TargetPositionOffset.y, 0);
                    }
                    break;
                default:
                    spawn_pos = v_attacker.transform.position + (v_attacker.transform.forward * expl_data.m_TargetPositionOffset.z) + new Vector3(0, expl_data.m_TargetPositionOffset.y, 0);
                    break;
            }

            Instantiate(expl_data.effectPrefab, spawn_pos, spawn_rot);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
