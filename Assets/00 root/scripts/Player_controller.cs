using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_controller : MonoBehaviour {
    #region Singleton
    private static Player_controller _instance = null;
    public static Player_controller Instance
    {
        get
        {
            return _instance;
        }
    }
    void Awake()
    {
        if(_instance == null)
        {
            _instance = GameObject.FindObjectOfType<Player_controller>();
        }else
        {
            GameObject.Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);

        Startup();
    }
    #endregion Singleton

    public BotController m_BotController;
    public ETInput m_ETInput;

    public void Startup()
    {
        m_ETInput = GetComponent<ETInput>();
        m_ETInput.m_PlayerController = this;

        // bot Data 설정 및 생성 후
        m_BotController = GetComponentInChildren<BotController>();
        m_BotController.SetPlayer();
    }
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
