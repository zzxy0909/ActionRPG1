#pragma strict
var questDataBase : GameObject;

var questProgress : int[] = new int[20];
var questSlot : int[] = new int[5];

var questNameText : GUIStyle;
var questDescriptionText : GUIStyle;

var skin1 :  GUISkin;
var windowRect : Rect = new Rect (360 ,140 ,480 ,490);
private var originalRect : Rect;

private var menu : boolean = false;


function Start () {
	var quest : QuestData = questDataBase.GetComponent(QuestData);
	// If Array Length of questProgress Variable < QuestData.Length
	if(questProgress .Length < quest.questData.Length){
		questProgress = new int[quest.questData.Length];
	}
	originalRect = windowRect;

}

function Update () {
	if (Input.GetKeyDown("q")) {
		OnOffMenu();
	}
	

}

function AddQuest(id : int) : boolean{
	var full : boolean = false;
	var geta : boolean = false;
	
	var pt = 0;
	while(pt < questSlot.Length && !geta){
		if(questSlot[pt] == id){
			print("You Already Accept this Quest");
			geta = true;
		
		}else if(questSlot[pt] == 0){
			questSlot[pt] = id;
			geta = true;
		}else{
			pt++;
			if(pt >= questSlot.Length){
				full = true;
				print("Full");
			}
		}
		
	}
	
	return full;

}

function SortQuest(){
		var pt = 0;
		var nextp = 0;
		var clearr : boolean = false;
	while(pt < questSlot.Length){
		if(questSlot[pt] == 0){
			nextp = pt + 1;
			while(nextp < questSlot.Length && !clearr){
				if(questSlot[nextp] > 0){
				//Fine Next Slot and Set
					questSlot[pt] = questSlot[nextp];
					questSlot[nextp] = 0;
					clearr = true;
				}else{
					nextp++;
				}
			
			}
		//Continue New Loop
			clearr = false;
			pt++;
		}else{
			pt++;
		}
		
	}

}

function OnGUI(){
	if(menu){
		windowRect = GUI.Window (5, windowRect, QuestWindow, "Quest Log");
		//------------------------
	}

}

function QuestWindow(windowID : int){
		GUI.skin = skin1;
		var data : QuestData = questDataBase.GetComponent(QuestData);
		//Close Window Button
		if (GUI.Button (Rect (390,8,70,70), "X")) {
			OnOffMenu();
		}
		//GUI.Box (Rect (260,140,280,385), "Quest Log");
		
		if(questSlot[0] > 0){
			//Quest Name
			GUI.Label (Rect (15, 45, 280, 40), data.questData[questSlot[0]].questName , questNameText);
			//Quest Info + Progress
			GUI.Label (Rect (15, 75, 280, 40), data.questData[questSlot[0]].description + " (" + questProgress[questSlot[0]].ToString() + " / " + data.questData[questSlot[0]].finishProgress + ")" , questDescriptionText);
			//Cancel Quest
			if (GUI.Button (Rect (290,45,84,50), "Cancel")) {
				questProgress[questSlot[0]] = 0;
				questSlot[0] = 0;
				SortQuest();
			}
		}
		//-----------------------------------------
		if(questSlot[1] > 0){
			//Quest Name
			GUI.Label (Rect (15, 125, 280, 40), data.questData[questSlot[1]].questName , questNameText);
			//Quest Info + Progress
			GUI.Label (Rect (15, 155, 280, 40), data.questData[questSlot[1]].description + " (" + questProgress[questSlot[1]].ToString() + " / " + data.questData[questSlot[1]].finishProgress + ")" , questDescriptionText);
			//Cancel Quest
			if (GUI.Button (Rect (290,125,84,50), "Cancel")) {
				questProgress[questSlot[1]] = 0;
				questSlot[1] = 0;
				SortQuest();
			}
		}
		//-----------------------------------------
		if(questSlot[2] > 0){
			//Quest Name
			GUI.Label (Rect (15, 205, 280, 40), data.questData[questSlot[2]].questName , questNameText);
			//Quest Info + Progress
			GUI.Label (Rect (15, 235, 280, 40), data.questData[questSlot[2]].description + " (" + questProgress[questSlot[2]].ToString() + " / " + data.questData[questSlot[2]].finishProgress + ")" , questDescriptionText);
			//Cancel Quest
			if (GUI.Button (Rect (290,205,84,50), "Cancel")) {
				questProgress[questSlot[2]] = 0;
				questSlot[2] = 0;
				SortQuest();
			}
		}
		//-----------------------------------------
		if(questSlot[3] > 0){
			//Quest Name
			GUI.Label (Rect (15, 285, 280, 40), data.questData[questSlot[3]].questName , questNameText);
			//Quest Info + Progress
			GUI.Label (Rect (15, 315, 280, 40), data.questData[questSlot[3]].description + " (" + questProgress[questSlot[3]].ToString() + " / " + data.questData[questSlot[3]].finishProgress + ")" , questDescriptionText);
			//Cancel Quest
			if (GUI.Button (Rect (290,285,84,50), "Cancel")) {
				questProgress[questSlot[3]] = 0;
				questSlot[3] = 0;
				SortQuest();
			}
		}
		//-----------------------------------------
		if(questSlot[4] > 0){
			//Quest Name
			GUI.Label (Rect (15, 365, 280, 40), data.questData[questSlot[4]].questName , questNameText);
			//Quest Info + Progress
			GUI.Label (Rect (15, 395, 280, 40), data.questData[questSlot[4]].description + " (" + questProgress[questSlot[4]].ToString() + " / " + data.questData[questSlot[4]].finishProgress + ")" , questDescriptionText);
			//Cancel Quest
			if (GUI.Button (Rect (290,365,84,50), "Cancel")) {
				questProgress[questSlot[4]] = 0;
				questSlot[4] = 0;
				SortQuest();
			}
		}
		
		//-----------------------------------------
		GUI.DragWindow (new Rect (0,0,10000,10000)); 
}

function OnOffMenu(){
	//Freeze Time Scale to 0 if Window is Showing
	if(!menu && Time.timeScale != 0.0){
			menu = true;
			ResetPosition();
			Time.timeScale = 0.0;
	}else if(menu){
			menu = false;
			Time.timeScale = 1.0;
	}

}

function Progress(id : int){
	//Check for You have a quest ID match to one of Quest Slot
	for(var n = 0; n < questSlot.Length ; n++){
		if(questSlot[n] == id && id != 0){
			var data : QuestData = questDataBase.GetComponent(QuestData);
			// Check If The Progress of this quest < Finish Progress then increase 1 of Quest Progress Variable
			if(questProgress[id] < data.questData[questSlot[n]].finishProgress){
				questProgress[id] += 1;
			}
			print("Quest Slot =" + n);
		}

    }
    

}
//-----------------------------------------------

function CheckQuestSlot(id : int) : boolean{
	//Check for You have a quest ID match to one of Quest Slot
	var exist : boolean = false;
	for(var n = 0; n < questSlot.Length ; n++){
		if(questSlot[n] == id && id != 0){
			//You Have this quest in the slot
			exist = true;
		}

    }
    return exist;

}

function CheckQuestProgress(id : int) : int{
	//Check for You have a quest ID match to one of Quest Slot
	var qProgress : int = 0;
	for(var n = 0; n < questSlot.Length ; n++){
		if(questSlot[n] == id && id != 0){
			//You Have this quest in the slot
			qProgress = questProgress[id];
		}

    }
    return qProgress;

}

//---------------------------------------

function Clear(id : int){
	//Check for You have a quest ID match to one of Quest Slot
	for(var n = 0; n < questSlot.Length ; n++){
		if(questSlot[n] == id && id != 0){
			var data : QuestData = questDataBase.GetComponent(QuestData);
				questProgress[id] += 10;
				questSlot[n] = 0;
				SortQuest();
			print("Quest Slot =" + n);
		}

    }
    
}

function ResetPosition(){
		//Reset GUI Position when it out of Screen.
		if(windowRect.x >= Screen.width -30 || windowRect.y >= Screen.height -30 || windowRect.x <= -70 || windowRect.y <= -70 ){
			windowRect = originalRect;
		}
}


