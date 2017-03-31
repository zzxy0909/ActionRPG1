#pragma strict
public var runMaxAnimationSpeed : float = 1.0;
public var backMaxAnimationSpeed : float = 1.0;
public var sprintAnimationSpeed : float = 1.5;

private var player : GameObject;
private var mainModel : GameObject;

//var idle : String = "idle";
var idle : AnimationClip;
var run : AnimationClip;
var right : AnimationClip;
var left : AnimationClip;
var back : AnimationClip;
var jump : AnimationClip;
var hurt : AnimationClip;
var joyStick : GameObject;

function Start () {
	if(!player){
		player = this.gameObject;
	}
	if(!joyStick){
		joyStick = GameObject.FindWithTag("JoyStick");
	}
	mainModel = GetComponent(Status).mainModel;
	if(!mainModel){
		mainModel = this.gameObject;
	}
	
	mainModel.GetComponent.<Animation>()[run.name].speed = runMaxAnimationSpeed;
	mainModel.GetComponent.<Animation>()[right.name].speed = runMaxAnimationSpeed;
	mainModel.GetComponent.<Animation>()[left.name].speed = runMaxAnimationSpeed;
	mainModel.GetComponent.<Animation>()[back.name].speed = backMaxAnimationSpeed;
	
	if(jump){
		mainModel.GetComponent.<Animation>()[jump.name].wrapMode  = WrapMode.ClampForever;
	}
	
	if(hurt){
		GetComponent(Status).hurt = hurt;
		mainModel.GetComponent.<Animation>()[hurt.name].layer = 5;
	}
	
}

function Update () {
	if(Input.GetButton("Horizontal") || Input.GetButton("Vertical")){
		var moveHorizontal : float = Input.GetAxis("Horizontal");
		var moveVertical : float = Input.GetAxis("Vertical");
	}else if(joyStick){
		moveHorizontal = joyStick.GetComponent(MobileJoyStick).position.x;
		moveVertical = joyStick.GetComponent(MobileJoyStick).position.y;
	}
	
    var controller : CharacterController = player.GetComponent(CharacterController);
    if ((controller.collisionFlags & CollisionFlags.Below) != 0){
        if (moveHorizontal > 0.1)
     		mainModel.GetComponent.<Animation>().CrossFade(right.name);
   		else if (moveHorizontal < -0.1)
      		mainModel.GetComponent.<Animation>().CrossFade(left.name);
   		else if (moveVertical > 0.1)
      		mainModel.GetComponent.<Animation>().CrossFade(run.name);
   		else if (moveVertical < -0.1)
      		mainModel.GetComponent.<Animation>().CrossFade(back.name);
   else
      mainModel.GetComponent.<Animation>().CrossFade(idle.name);
	}else{
		if(jump){
			mainModel.GetComponent.<Animation>().CrossFade(jump.name);
		}
		
	}
}

function AnimationSpeedSet(){
		mainModel = GetComponent(AttackTrigger).mainModel;
		if(!mainModel){
			mainModel = this.gameObject;
		}
		mainModel.GetComponent.<Animation>()[run.name].speed = runMaxAnimationSpeed;
		mainModel.GetComponent.<Animation>()[right.name].speed = runMaxAnimationSpeed;
		mainModel.GetComponent.<Animation>()[left.name].speed = runMaxAnimationSpeed;
		mainModel.GetComponent.<Animation>()[back.name].speed = backMaxAnimationSpeed;
}

@script RequireComponent (AttackTrigger)
