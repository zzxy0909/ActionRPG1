using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class ETInput : MonoBehaviour {
    
    public NavMeshAgent m_agent;
    public Player_controller m_PlayerController;

	void OnEnable(){
		EasyJoystick.On_JoystickMove += On_JoystickMove;	
		EasyJoystick.On_JoystickMoveEnd += On_JoystickMoveEnd;
		//EasyButton.On_ButtonPress += On_ButtonPress;
		//EasyButton.On_ButtonUp += On_ButtonUp;	
		//EasyButton.On_ButtonDown += On_ButtonDown;
	}

	void Fire(){
		//if (buttonName=="Fire"){
		//	Instantiate( bullet, gun.transform.position, gun.rotation);
		//}		
	}


	void OnDisable(){
		EasyJoystick.On_JoystickMove -= On_JoystickMove;	
		EasyJoystick.On_JoystickMoveEnd -= On_JoystickMoveEnd;
//		EasyButton.On_ButtonPress -= On_ButtonPress;
		//EasyButton.On_ButtonUp -= On_ButtonUp;	
	}
		
	void OnDestroy(){
		EasyJoystick.On_JoystickMove -= On_JoystickMove;	
		EasyJoystick.On_JoystickMoveEnd -= On_JoystickMoveEnd;
//		EasyButton.On_ButtonPress -= On_ButtonPress;
		//EasyButton.On_ButtonUp -= On_ButtonUp;	
	}
	
	void Start(){
        
        if (m_agent == null)
            m_agent = GetComponent<NavMeshAgent>();

    }
	void MoveProc(MovingJoystick move)
    {
        float angle = move.Axis2Angle(true);
        transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        // transform.Translate(Vector3.forward * move.joystickValue.magnitude * Time.deltaTime);	
        Vector2 joystickMove = move.joystickAxis;
        joystickMove.Normalize();
        // top view
        Vector3 joystickWorldDirection = Camera.main.transform.TransformDirection(new Vector3(joystickMove.x, 0, joystickMove.y)).normalized;
        Vector3 resultPos = transform.position + joystickWorldDirection; // *0.05f; // * 100.0f; // Need : test     

        // first view
//???        Vector3 resultPos = transform.position + transform.forward * joystickMove.y; // *0.05f; // * 100.0f; // Need : test     


        // Vector3 resultPos = transform.forward * move.joystickValue.magnitude * 0.1f; // Time.deltaTime;
        m_agent.SetDestination(resultPos);
        m_agent.Resume();
        m_PlayerController.m_BotController.SetRun();
    }
	void On_JoystickMove( MovingJoystick move){
	
        if(m_PlayerController.m_BotController.m_Move_stop_option.checkStopCase() == false)
        {
            MoveProc(move);
        }
        // 연속 공격에 방향 전환 하지않고 대쉬 공격위주로 처리함.
        if(m_PlayerController.m_BotController.m_Move_stop_option.m_autoDash == true)
        {

        }
        else if (m_PlayerController.m_BotController.m_Move_stop_option.m_autoDash == false
            && m_PlayerController.m_BotController.m_Move_stop_option.m_isPlaySkillorAttack == true
            && m_PlayerController.m_BotController.m_Move_stop_option.m_isPlayAvoid == false
            )
        {
            float angle = move.Axis2Angle(true);
            transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }
    }
    void StopProc()
    {
        m_agent.Stop();
        m_PlayerController.m_BotController.SetIdle();
    }
	void On_JoystickMoveEnd (MovingJoystick move)
	{
        m_PlayerController.m_BotController.SetRun_end(); // 우선 이동 상태는 종료

        if (m_PlayerController.m_BotController.m_Move_stop_option.checkStopCase() == false) // skill 시전 중 혹은 회피 중 등등 이 아닐때
        {
            StopProc();
        }
    }
	
	/*
	void On_ButtonPress (string buttonName)
	{
		if (buttonName=="Fire"){
			Instantiate( bullet, gun.transform.position, gun.rotation);
		}
	}*/
	
	//void On_ButtonUp (string buttonName)
	//{
	//	if (buttonName=="Exit"){
	//		Application.LoadLevel("StartMenu");	
	//	}
	//}	
}
