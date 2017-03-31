#pragma strict
private var show : boolean = false;
var textStyle : GUIStyle;
var textStyle2 : GUIStyle;

//Icon for Buffs
var braveIcon : Texture2D;
var barrierIcon : Texture2D;
var faithIcon : Texture2D;
var magicBarrierIcon : Texture2D;

var windowRect : Rect = new Rect (250, 220, 550, 450);
private var originalRect : Rect;

function Start(){
	originalRect = windowRect;
}

function Update () {
	if(Input.GetKeyDown("c")){
		OnOffMenu();
	}
}

function  OnGUI (){
		var stat : Status = GetComponent(Status);
		if(show){
			windowRect = GUI.Window (0, windowRect, StatWindow, "Status");
			
		}
		
		//Show Buffs Icon
		if(stat.brave){
			GUI.DrawTexture( new Rect(360,50,40,40), braveIcon);
		}
		if(stat.barrier){
			GUI.DrawTexture( new Rect(400,50,40,40), barrierIcon);
		}
		if(stat.faith){
			GUI.DrawTexture( new Rect(440,50,40,40), faithIcon);
		}
		if(stat.mbarrier){
			GUI.DrawTexture( new Rect(480,50,40,40), magicBarrierIcon);
		}
	}
	
function StatWindow(windowID : int){
			var stat : Status = GetComponent(Status);
			//GUI.Box ( new Rect(180,170,240,380), "Status");
			GUI.Label ( new Rect(20, 40, 100, 50), "Level" , textStyle);
			GUI.Label ( new Rect(20, 100, 100, 50), "STR" , textStyle);
			GUI.Label ( new Rect(270, 100, 100, 50), "DEF" , textStyle);
			GUI.Label ( new Rect(20, 190, 100, 50), "MATK" , textStyle);
			GUI.Label ( new Rect(270, 190, 100, 50), "MDEF" , textStyle);
			
			GUI.Label ( new Rect(20, 300, 100, 50), "EXP" , textStyle);
			GUI.Label ( new Rect(20, 350, 150, 50), "Next LV" , textStyle);
			GUI.Label ( new Rect(20, 400, 200, 50), "Status Point" , textStyle);
			//Close Window Button
			if (GUI.Button ( new Rect(470,5,70,70), "X")) {
				OnOffMenu();
			}
			//Status
			var lv : int = stat.level;
			var atk : int = stat.atk;
			var def : int = stat.def;
			var matk : int = stat.matk;
			var mdef : int = stat.mdef;
			var exp : int = stat.exp;
			var next : int = stat.maxExp - exp;
			var stPoint : int = stat.statusPoint;
			
			GUI.Label ( new Rect(50, 40, 100, 50), lv.ToString() , textStyle2);
			GUI.Label ( new Rect(80, 100, 100, 50), atk.ToString() , textStyle2);
			GUI.Label ( new Rect(310, 100, 100, 50), def.ToString() , textStyle2);
			GUI.Label ( new Rect(80, 190, 100, 50), matk.ToString() , textStyle2);
			GUI.Label ( new Rect(310, 190, 100, 50), mdef.ToString() , textStyle2);
			
			GUI.Label ( new Rect(320, 300, 100, 50), exp.ToString() , textStyle2);
			GUI.Label ( new Rect(320, 350, 100, 50), next.ToString() , textStyle2);
			GUI.Label ( new Rect(320, 400, 100, 50), stPoint.ToString() , textStyle2);
			
			if (GUI.Button ( new Rect(210,90,50,50), "+") && stPoint > 0) {
				GetComponent(Status).atk += 1;
				GetComponent(Status).statusPoint -= 1;
				GetComponent(Status).CalculateStatus();
			}
			if (GUI.Button ( new Rect(430,90,50,50), "+") && stPoint > 0) {
				GetComponent(Status).def += 1;
				GetComponent(Status).maxHealth += 5;
				GetComponent(Status).statusPoint -= 1;
				GetComponent(Status).CalculateStatus();
			}
			if (GUI.Button ( new Rect(210,180,50,50), "+") && stPoint > 0) {
				GetComponent(Status).matk += 1;
				GetComponent(Status).maxMana += 3;
				GetComponent(Status).statusPoint -= 1;
				GetComponent(Status).CalculateStatus();
			}
			if (GUI.Button ( new Rect(430,180,50,50), "+") && stPoint > 0) {
				GetComponent(Status).mdef += 1;
				GetComponent(Status).statusPoint -= 1;
				GetComponent(Status).CalculateStatus();
			}
			GUI.DragWindow (new Rect (0,0,10000,10000)); 
	}

function OnOffMenu(){
	//Freeze Time Scale to 0 if Status Window is Showing
	if(!show && Time.timeScale != 0.0){
			show = true;
			Time.timeScale = 0.0;
			ResetPosition();
			Screen.lockCursor = false;
	}else if(show){
			show = false;
			Time.timeScale = 1.0;
			//Screen.lockCursor = true;
	}
}

function ResetPosition(){
		//Reset GUI Position when it out of Screen.
		if(windowRect.x >= Screen.width -30 || windowRect.y >= Screen.height -30 || windowRect.x <= -70 || windowRect.y <= -70 ){
			windowRect = originalRect;
		}
		
}

@script RequireComponent (Status)