using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour {
    #region Singleton
    private static AiManager _instance = null;
    public static AiManager Instance
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
            _instance = this; // GameObject.FindObjectOfType<AiManager>();
        }
        else
        {
            GameObject.Destroy(this.gameObject);
            return;
        }
        
    }
    #endregion Singleton

    public List<AiFriend_controller> m_ListAiFriend = new List<AiFriend_controller>();
    public List<AiEnemy_controller> m_ListAiEnemy = new List<AiEnemy_controller>();

    void ClearAll()
    {
        m_ListAiFriend.Clear();
        m_ListAiEnemy.Clear();
    }

    public void Add_AiEnemy(AiEnemy_controller v_enemy)
    {
        m_ListAiEnemy.Add(v_enemy);
    }
    public void Add_AiFriend(AiFriend_controller v_enemy)
    {
        m_ListAiFriend.Add(v_enemy);
    }

    // Use this for initialization
    void Start () {
        ClearAll();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public AiEnemy_controller Get_NearAiEnemy(Vector3 pos)
    {
        float nearValue = 999f;
        int ix = -1;
        for (int i=0; i<m_ListAiEnemy.Count; i++ )
        {
            if(m_ListAiEnemy[i].gameObject.activeSelf == true)
            {
                float tmp_dist = (pos - m_ListAiEnemy[i].transform.position).magnitude;
                if (tmp_dist < nearValue)
                {
                    ix = i;
                    nearValue = tmp_dist;
                }
            }
        }

        if(ix >= 0)
        {
            return m_ListAiEnemy[ix];
        }
        else
        {
            return null;
        }
    }
}
