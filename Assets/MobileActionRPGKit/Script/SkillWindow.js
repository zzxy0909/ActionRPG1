#pragma strict
var player : GameObject;
var database : GameObject;

var skill : int[] = new int[3];
var skillListSlot : int[] = new int[15];

class LearnSkillLV{
	var level : int = 1;
	var skillId : int = 1;
}
var learnSkill : LearnSkillLV[] = new LearnSkillLV[2];

private var menu : boolean = false;
private var shortcutPage : boolean = true;
private var skillListPage : boolean = false;
private var skillSelect : int = 0;

var skin1 :  GUISkin;
var windowRect : Rect = new Rect (360 ,80 ,360 ,185);
private var originalRect : Rect;

var skillNameText : GUIStyle;
var skillDescriptionText : GUIStyle;
var showLearnSkillText : GUIStyle;

private var showSkillLearned : boolean = false;
private var showSkillName : String = "";

var skillButton : GameObject[] = new GameObject[3];
var pageMultiply : int = 5;
private var page : int = 0;

function Start () {
		if(!player){
			player = this.gameObject;
		}
		originalRect = windowRect;
		//AssignAllSkill();

}

function Update () {
	if (Input.GetKeyDown("k")) {
		OnOffMenu();
	}

}

function OnOffMenu(){
	//Freeze Time Scale to 0 if Window is Showing
	if(!menu && Time.timeScale != 0.0){
			menu = true;
			skillListPage = false;
			shortcutPage = true;
			Time.timeScale = 0.0;
			ResetPosition();
			Screen.lockCursor = false;
	}else if(menu){
			menu = false;
			Time.timeScale = 1.0;
			//Screen.lockCursor = true;
	}
}

function OnGUI(){
	var dataItem : SkillData = database.GetComponent(SkillData);
	if(showSkillLearned){
		GUI.Label (Rect (Screen.width /2 -50, 85, 400, 50), "You Learned  " + showSkillName , showLearnSkillText);
	}
	if(menu && shortcutPage){
		windowRect = GUI.Window (3, windowRect, SkillShortcut, "Skill");
	}
	//---------------Skill List----------------------------
	if(menu && skillListPage){
		windowRect = GUI.Window (3, windowRect, AllSkill, "Skill");
	}
	
}

function AssignSkill(id : int , sk : int){
	var dataSkill : SkillData = database.GetComponent(SkillData);
	player.GetComponent(AttackTrigger).skill[id].manaCost = dataSkill.skill[skillListSlot[sk]].manaCost;
	player.GetComponent(AttackTrigger).skill[id].skillPrefab = dataSkill.skill[skillListSlot[sk]].skillPrefab;
	player.GetComponent(AttackTrigger).skill[id].skillAnimation = dataSkill.skill[skillListSlot[sk]].skillAnimation;
	player.GetComponent(AttackTrigger).skill[id].icon = dataSkill.skill[skillListSlot[sk]].icon;
	skill[id] = skillListSlot[sk];
	//-------Assign Icon to Skill Button-----------
	if(skillButton[id]){
		skillButton[id].GetComponent.<GUITexture>().texture = dataSkill.skill[skillListSlot[sk]].icon;
		skillButton[id].GetComponent(SkillButton).originalTexture = dataSkill.skill[skillListSlot[sk]].icon;
		//if(dataSkill.skill[skillListSlot[sk]].iconDown){
			skillButton[id].GetComponent(SkillButton).downTexture = dataSkill.skill[skillListSlot[sk]].iconDown;
		//}
	}
	print(sk);

}

function AssignAllSkill(){
		/*AssignSkill(0 , skill[0]);
		AssignSkill(1 , skill[1]);
		AssignSkill(2 , skill[2]);*/
	if(!player){
			player = this.gameObject;
		}
	var n : int = 0;
	var dataSkill : SkillData = database.GetComponent(SkillData);
	while(n <= 2){
		player.GetComponent(AttackTrigger).skill[n].manaCost = dataSkill.skill[skill[n]].manaCost;
		player.GetComponent(AttackTrigger).skill[n].skillPrefab = dataSkill.skill[skill[n]].skillPrefab;
		player.GetComponent(AttackTrigger).skill[n].skillAnimation = dataSkill.skill[skill[n]].skillAnimation;
		player.GetComponent(AttackTrigger).skill[n].icon = dataSkill.skill[skill[n]].icon;
		//-------Assign Icon to Skill Button-----------
		if(skillButton[n]){
			skillButton[n].GetComponent.<GUITexture>().texture = dataSkill.skill[skill[n]].icon;
			skillButton[n].GetComponent(SkillButton).originalTexture = dataSkill.skill[skill[n]].icon;
			//if(dataSkill.skill[skill[n]].iconDown){
				skillButton[n].GetComponent(SkillButton).downTexture = dataSkill.skill[skill[n]].iconDown;
			//}
		}
		n++;
	}

}

function SkillShortcut(windowID : int){
		var dataSkill : SkillData = database.GetComponent(SkillData);
		windowRect.width = 490;
		windowRect.height = 275;
		//Close Window Button
		if (GUI.Button (Rect (400,8,65,65), "X")) {
			OnOffMenu();
		}
		
		//Skill Shortcut
		if (GUI.Button (Rect (40,85,120,120), dataSkill.skill[skill[0]].icon)) {
			skillSelect = 0;
			skillListPage = true;
			shortcutPage = false;
		}
		GUI.Label (Rect (90, 210, 20, 20), "1");
		if (GUI.Button (Rect (190,85,120,120), dataSkill.skill[skill[1]].icon)) {
			skillSelect = 1;
			skillListPage = true;
			shortcutPage = false;
		}
		GUI.Label (Rect (245, 210, 20, 20), "2");
		if (GUI.Button (Rect (340,85,120,120), dataSkill.skill[skill[2]].icon)) {
			skillSelect = 2;
			skillListPage = true;
			shortcutPage = false;
		}
		GUI.Label (Rect (400, 210, 20, 20), "3");
		
		GUI.DragWindow (new Rect (0,0,10000,10000));

}

function AllSkill(windowID : int){
		var dataSkill : SkillData = database.GetComponent(SkillData);
		windowRect.width = 400;
		windowRect.height = 575;
		//Close Window Button
		if (GUI.Button (Rect (310,8,70,70), "X")) {
			OnOffMenu();
		}
		if (GUI.Button (Rect (20,60,75,75), dataSkill.skill[skillListSlot[0 + page]].icon)) {
			AssignSkill(skillSelect , 0 + page);
			shortcutPage = true;
			skillListPage = false;
			
		}
		GUI.Label (Rect (110, 70, 140, 40), dataSkill.skill[skillListSlot[0 + page]].skillName , skillNameText); //Show Skill's Name
		GUI.Label (Rect (110, 95, 140, 40), dataSkill.skill[skillListSlot[0 + page]].description , skillDescriptionText); //Show Skill's Description
		GUI.Label (Rect (310, 90, 140, 40), "MP : " + dataSkill.skill[skillListSlot[0 + page]].manaCost , skillDescriptionText); //Show Skill's MP Cost
		//-----------------------------
		if (GUI.Button (Rect (20,150,75,75), dataSkill.skill[skillListSlot[1 + page]].icon)) {
			AssignSkill(skillSelect , 1 + page);
			shortcutPage = true;
			skillListPage = false;
			
		}
		GUI.Label (Rect (110, 160, 140, 40), dataSkill.skill[skillListSlot[1 + page]].skillName , skillNameText); //Show Skill's Name
		GUI.Label (Rect (110, 185, 140, 40), dataSkill.skill[skillListSlot[1 + page]].description , skillDescriptionText); //Show Skill's Description
		GUI.Label (Rect (310, 180, 140, 40), "MP : " + dataSkill.skill[skillListSlot[1 + page]].manaCost , skillDescriptionText); //Show Skill's MP Cost
		//-----------------------------
		if (GUI.Button (Rect (20,240,75,75), dataSkill.skill[skillListSlot[2 + page]].icon)) {
			AssignSkill(skillSelect , 2 + page);
			shortcutPage = true;
			skillListPage = false;
			
		}
		GUI.Label (Rect (110, 250, 140, 40), dataSkill.skill[skillListSlot[2 + page]].skillName , skillNameText); //Show Skill's Name
		GUI.Label (Rect (110, 275, 140, 40), dataSkill.skill[skillListSlot[2 + page]].description , skillDescriptionText); //Show Skill's Description
		GUI.Label (Rect (310, 270, 140, 40), "MP : " + dataSkill.skill[skillListSlot[2 + page]].manaCost , skillDescriptionText); //Show Skill's MP Cost
		//-----------------------------
		if (GUI.Button (Rect (20,330,75,75), dataSkill.skill[skillListSlot[3 + page]].icon)) {
			AssignSkill(skillSelect , 3 + page);
			shortcutPage = true;
			skillListPage = false;
			
		}
		GUI.Label (Rect (110, 340, 140, 40), dataSkill.skill[skillListSlot[3 + page]].skillName , skillNameText); //Show Skill's Name
		GUI.Label (Rect (110, 365, 140, 40), dataSkill.skill[skillListSlot[3 + page]].description , skillDescriptionText); //Show Skill's Description
		GUI.Label (Rect (310, 360, 140, 40), "MP : " + dataSkill.skill[skillListSlot[3 + page]].manaCost , skillDescriptionText); //Show Skill's MP Cost
		//-----------------------------
		if (GUI.Button (Rect (20,420,75,75), dataSkill.skill[skillListSlot[4 + page]].icon)) {
			AssignSkill(skillSelect , 4 + page);
			shortcutPage = true;
			skillListPage = false;
			
		}
		GUI.Label (Rect (110, 430, 140, 40), dataSkill.skill[skillListSlot[4 + page]].skillName , skillNameText); //Show Skill's Name
		GUI.Label (Rect (110, 455, 140, 40), dataSkill.skill[skillListSlot[4 + page]].description , skillDescriptionText); //Show Skill's Description
		GUI.Label (Rect (310, 450, 140, 40), "MP : " + dataSkill.skill[skillListSlot[4 + page]].manaCost , skillDescriptionText); //Show Skill's MP Cost
		//-----------------------------
		
		
		if (GUI.Button (Rect (150,515,50,52), "1")) {
			page = 0;
		}
		if (GUI.Button (Rect (220,515,50,52), "2")) {
			page = pageMultiply;
		}
		if (GUI.Button (Rect (290,515,50,52), "3")) {
			page = pageMultiply *2;
		}
		
		GUI.DragWindow (new Rect (0,0,10000,10000));
}

function AddSkill(id : int){
	var geta : boolean = false;
	var pt : int = 0;
	while(pt < skillListSlot.Length && !geta){
		if(skillListSlot[pt] == id){
			// Check if you already have this skill.
			geta = true;
		}else if(skillListSlot[pt] == 0){
			// Add Skill to empty slot.
			skillListSlot[pt] = id;
			ShowLearnedSkill(id);
			geta = true;
		}else{
			pt++;
		}
		
	}
	
}

function ShowLearnedSkill(id : int){
	var dataSkill : SkillData = database.GetComponent(SkillData);
	showSkillLearned = true;
	showSkillName = dataSkill.skill[id].skillName;
	yield WaitForSeconds(10.5);
	showSkillLearned = false;

}

function ResetPosition(){
		//Reset GUI Position when it out of Screen.
		if(windowRect.x >= Screen.width -30 || windowRect.y >= Screen.height -30 || windowRect.x <= -70 || windowRect.y <= -70 ){
			windowRect = originalRect;
		}
}

function LearnSkillByLevel(lv : int){
	var c : int = 0;
	while(c < learnSkill.Length){
		if(lv >= learnSkill[c].level){
			AddSkill(learnSkill[c].skillId);
		}
		c++;
	}

}