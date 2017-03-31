using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_controller : MonoBehaviour {
    public BotController m_BotController;
    public ETInput m_ETInput;

    public void Startup()
    {
        m_ETInput = GetComponent<ETInput>();
        m_ETInput.m_PlayerController = this;

        // bot Data 설정 및 생성 후
        m_BotController = GetComponentInChildren<BotController>();
    }
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
