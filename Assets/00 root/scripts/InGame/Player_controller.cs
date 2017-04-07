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
            _instance = this; // GameObject.FindObjectOfType<Player_controller>();
        }else
        {
            GameObject.Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);

        
    }
    #endregion Singleton

    public BotController m_BotController;
    public UI_BotUI m_BotUI;
    public ETInput m_ETInput;
    public StatusNbuff m_StatusNbuff;

    public void Startup()
    {
        m_ETInput = GetComponent<ETInput>();
        m_ETInput.m_PlayerController = this;

        // bot Data 설정 및 생성 후
        m_BotController = GetComponentInChildren<BotController>();
        StartCoroutine( m_BotController.SetPlayer(m_BotUI) );

    }
    // Use this for initialization
    void Start () {
        Startup();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
