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
    public Skill_controller m_Skill_controller;
    public equipController m_equipController;
    public StatusNbuff m_StatusNbuff;

    public bool m_StartupOK = false;

    public void Startup()
    {
        m_ETInput = GetComponent<ETInput>();
        m_ETInput.m_PlayerController = this;

        // bot Data 설정 및 생성 후
        m_BotController = GetComponentInChildren<BotController>();
        StartCoroutine( SetPlayer() );

    }
    // Use this for initialization
    void Start () {
        Startup();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    Gui_BotUIRoot m_Gui_BotUIRoot;
    public IEnumerator SetPlayer()
    {
        m_BotController.m_Bot_type = eBot_type.player;

        while (m_Gui_BotUIRoot == null)
        {
            m_Gui_BotUIRoot = GuiMgr.Instance.Find<Gui_BotUIRoot>();
            yield return new WaitForEndOfFrame();
        }

        // BotUI 설정.
        if (m_BotUI == null)
        {
            m_BotUI = GetComponent<UI_BotUI>();
        }
        m_BotUI.Init_UI_Info(m_BotController.m_posHPbar);
        

        if (m_BotController.m_controllerAgent == null)
        {
            m_BotController.m_controllerAgent = m_ETInput.m_agent; // GetComponent<NavMeshAgent>();
        }


        m_StartupOK = true;
    }
}
