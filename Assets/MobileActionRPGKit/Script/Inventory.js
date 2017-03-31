#pragma strict
private var menu : boolean = false;
private var itemMenu : boolean = true;
private var equipMenu : boolean = false;

var itemSlot : int[] = new int[15];
var itemQuantity : int[] = new int[15];
var equipment : int[] = new int[12];

var weaponEquip : int = 0;
var allowWeaponUnequip : boolean = false;
var armorEquip : int = 0;
var allowArmorUnequip : boolean = true;

var weapon : GameObject[] = new GameObject[1];

var player : GameObject;
var database : GameObject;
var fistPrefab : GameObject;

var cash : int = 500;

var skin : GUISkin;
var windowRect : Rect = new Rect (360 ,140 ,480 ,550);
private var originalRect : Rect;

var itemPageMultiply : int = 5;
var equipmentPageMultiply : int = 4;
private var page : int = 0;

var itemNameText : GUIStyle;
var itemDescriptionText : GUIStyle;
var itemQuantityText : GUIStyle;

function Start () {
		if(!player){
			player = this.gameObject;
		}
	var dataItem : ItemData = database.GetComponent(ItemData);
	originalRect = windowRect;
	SetEquipmentStatus();
}

function SetEquipmentStatus(){
		var dataItem : ItemData = database.GetComponent(ItemData);
		//Reset Power of Current Weapon & Armor
		player.GetComponent(Status).addAtk = 0;
		player.GetComponent(Status).addDef = 0;
		player.GetComponent(Status).addMatk = 0;
		player.GetComponent(Status).addMdef = 0;
		player.GetComponent(Status).weaponAtk = 0;
		player.GetComponent(Status).weaponMatk = 0;
		player.GetComponent(Status).addHPpercent = 0;
		player.GetComponent(Status).addMPpercent = 0;
		//Set New Variable of Weapon
		player.GetComponent(Status).weaponAtk += dataItem.equipment[weaponEquip].attack;
		player.GetComponent(Status).addDef += dataItem.equipment[weaponEquip].defense;
		player.GetComponent(Status).weaponMatk += dataItem.equipment[weaponEquip].magicAttack;
		player.GetComponent(Status).addMdef += dataItem.equipment[weaponEquip].magicDefense;
		//Set New Variable of Armor
		player.GetComponent(Status).weaponAtk += dataItem.equipment[armorEquip].attack;
		player.GetComponent(Status).addDef += dataItem.equipment[armorEquip].defense;
		player.GetComponent(Status).weaponMatk += dataItem.equipment[armorEquip].magicAttack;
		player.GetComponent(Status).addMdef += dataItem.equipment[armorEquip].magicDefense;
		
		player.GetComponent(Status).CalculateStatus();
}

function Update () {
	if (Input.GetKeyDown("i") || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
		OnOffMenu();
		//AutoSortItem();
	}
}

function UseItem(id : int){
	var dataItem : ItemData = database.GetComponent(ItemData);
	player.GetComponent(Status).Heal(dataItem.usableItem[id].hpRecover , dataItem.usableItem[id].mpRecover);
	player.GetComponent(Status).atk += dataItem.usableItem[id].atkPlus;
	player.GetComponent(Status).def += dataItem.usableItem[id].defPlus;
	player.GetComponent(Status).matk += dataItem.usableItem[id].matkPlus;
	player.GetComponent(Status).mdef += dataItem.usableItem[id].mdefPlus;
	
	AutoSortItem();
	
}

function EquipItem(id : int , slot : int){
	if(id == 0){
		return;
	}
	if(!player){
			player = this.gameObject;
		}
	var dataItem : ItemData = database.GetComponent(ItemData);
	//Backup Your Current Equipment before Unequip
	var tempEquipment : int = 0;
	
	if(dataItem.equipment[id].EquipmentType == dataItem.equipment[id].EqType.Weapon){
		//Weapon Type
		tempEquipment = weaponEquip;
		weaponEquip = id;
		if(dataItem.equipment[id].attackPrefab){
			player.GetComponent(AttackTrigger).attackPrefab = dataItem.equipment[id].attackPrefab.transform;
		}
		//Change Weapon Mesh
	if(dataItem.equipment[id].model && weapon){
		var allWeapon : int = weapon.length;
		var a : int = 0;
		if(allWeapon > 0 && dataItem.equipment[id].assignAllWeapon){
			while(a < allWeapon && weapon[a]){
					//weapon[a].SetActiveRecursively(true);
					weapon[a].SetActive(true);
				    var wea : GameObject = Instantiate(dataItem.equipment[id].model,weapon[a].transform.position,weapon[a].transform.rotation);
   					wea.transform.parent = weapon[a].transform.parent;
  					Destroy(weapon[a].gameObject);
  					weapon[a] = wea;
				a++;
			}
		}else if(allWeapon > 0){
			while(a < allWeapon && weapon[a]){
				if(a == 0){
					//weapon[a].SetActiveRecursively(true);
					weapon[a].SetActive(true);
				    wea = Instantiate(dataItem.equipment[id].model,weapon[a].transform.position,weapon[a].transform.rotation);
   					wea.transform.parent = weapon[a].transform.parent;
  					Destroy(weapon[a].gameObject);
  					weapon[a] = wea;
				}else{
					//weapon[a].SetActiveRecursively(false);
					weapon[a].SetActive(false);
				}
				a++;
			}
		}
	}
	}else{
		//Armor Type
		tempEquipment = armorEquip;
		armorEquip = id;
	}
	if(slot <= equipment.Length){
		equipment[slot] = 0;
	}
	//Assign Weapon Animation to PlayerAnimation Script
	AssignWeaponAnimation(id);
	//Reset Power of Current Weapon & Armor
	SetEquipmentStatus();
	
	AutoSortEquipment();
	AddEquipment(tempEquipment);

}

function RemoveWeaponMesh(){
			if(weapon){
				var allWeapon : int = weapon.length;
				var a : int = 0;
				if(allWeapon > 0){
					while(a < allWeapon && weapon[a]){
  							//weapon[a].SetActiveRecursively(false);
  							weapon[a].SetActive(false);
  							//Destroy(weapon[a].gameObject);
							a++;
					}
				}
			}
}

function UnEquip(id : int){
	var dataItem : ItemData = database.GetComponent(ItemData);
	if(!player){
		player = this.gameObject;
	}
	if(dataItem.equipment[id].model && weapon){
			var full : boolean = AddEquipment(weaponEquip);
	}else{
			full = AddEquipment(armorEquip);
	}
	if(!full){
		if(dataItem.equipment[id].model && weapon){
			weaponEquip = 0;
			player.GetComponent(AttackTrigger).attackPrefab = fistPrefab.transform;
			if(weapon){
				var allWeapon : int = weapon.length;
				var a : int = 0;
				if(allWeapon > 0){
					while(a < allWeapon && weapon[a]){
  							//weapon[a].SetActiveRecursively(false);
  							weapon[a].SetActive(false);
  							//Destroy(weapon[a].gameObject);
							a++;
					}
				}
			}
		}else{
			armorEquip = 0;
		}
	SetEquipmentStatus();
    	}
}

function  OnGUI (){
		GUI.skin = skin;
		if(menu && itemMenu){
			windowRect = GUI.Window (1, windowRect, ItemWindow, "Items");
		}
		if(menu && equipMenu){
			windowRect = GUI.Window (1, windowRect, ItemWindow, "Equipment");
		}
		
		if(menu){
			if (GUI.Button ( new Rect(windowRect.x -50, windowRect.y +105,50,100), "Item")) {
				//Switch to Item Tab
				page = 0;
				itemMenu = true;
				equipMenu = false;
			}
			if (GUI.Button ( new Rect(windowRect.x -50, windowRect.y +225,50,100), "Equip")) {
				//Switch to Equipment Tab
				page = 0;
				equipMenu = true;
				itemMenu = false;	
			}
		}
	}
	
//-----------Item Window-------------
function ItemWindow(windowID : int){
		var dataItem : ItemData = database.GetComponent(ItemData);
		if(menu && itemMenu){
			//GUI.Box ( new Rect(260,140,280,385), "Items");
			//Close Window Button
			if (GUI.Button ( new Rect(390,8,70,70), "X")) {
				OnOffMenu();
			}
			//Items Slot
			if (GUI.Button ( new Rect(30,30,75,75),dataItem.usableItem[itemSlot[0 + page]].icon)){
				UseItem(itemSlot[0 + page]);
				if(itemQuantity[0 + page] > 0){
					itemQuantity[0 + page]--;
				}
				if(itemQuantity[0 + page] <= 0){
					itemSlot[0 + page] = 0;
					itemQuantity[0 + page] = 0;
					AutoSortItem();
				}
			}
			GUI.Label ( new Rect(125, 40, 320, 75), dataItem.usableItem[itemSlot[0 + page]].itemName.ToString() , itemNameText); //Item Name
			GUI.Label ( new Rect(125, 65, 320, 75), dataItem.usableItem[itemSlot[0 + page]].description.ToString() , itemDescriptionText); //Item Description
			if(itemQuantity[0 + page] > 0){
				GUI.Label ( new Rect(88, 88, 40, 30), itemQuantity[0 + page].ToString() , itemQuantityText); //Quantity
			}
			//------------------------------------------------------
			if (GUI.Button ( new Rect(30,120,75,75),dataItem.usableItem[itemSlot[1 + page]].icon)){
				UseItem(itemSlot[1 + page]);
				if(itemQuantity[1 + page] > 0){
					itemQuantity[1 + page]--;
				}
				if(itemQuantity[1 + page] <= 0){
					itemSlot[1 + page] = 0;
					itemQuantity[1 + page] = 0;
					AutoSortItem();
				}
			}
			GUI.Label ( new Rect(125, 130, 320, 75), dataItem.usableItem[itemSlot[1 + page]].itemName.ToString() , itemNameText); //Item Name
			GUI.Label ( new Rect(125, 155, 320, 75), dataItem.usableItem[itemSlot[1 + page]].description.ToString() , itemDescriptionText); //Item Description
			if(itemQuantity[1 + page] > 0){
				GUI.Label ( new Rect(88, 178, 40, 30), itemQuantity[1 + page].ToString() , itemQuantityText); //Quantity
			}
			//------------------------------------------------------
			if (GUI.Button ( new Rect(30,210,75,75),dataItem.usableItem[itemSlot[2 + page]].icon)){
				UseItem(itemSlot[2 + page]);
				if(itemQuantity[2 + page] > 0){
					itemQuantity[2 + page]--;
				}
				if(itemQuantity[2 + page] <= 0){
					itemSlot[2 + page] = 0;
					itemQuantity[2 + page] = 0;
					AutoSortItem();
				}
			}
			GUI.Label ( new Rect(125, 220, 320, 75), dataItem.usableItem[itemSlot[2 + page]].itemName.ToString() , itemNameText); //Item Name
			GUI.Label ( new Rect(125, 245, 320, 75), dataItem.usableItem[itemSlot[2 + page]].description.ToString() , itemDescriptionText); //Item Description
			if(itemQuantity[2 + page] > 0){
				GUI.Label ( new Rect(88, 268, 40, 30), itemQuantity[2 + page].ToString() , itemQuantityText); //Quantity
			}
			//------------------------------------------------------
			if (GUI.Button ( new Rect(30,300,75,75),dataItem.usableItem[itemSlot[3 + page]].icon)){
				UseItem(itemSlot[3 + page]);
				if(itemQuantity[3 + page] > 0){
					itemQuantity[3 + page]--;
				}
				if(itemQuantity[3 + page] <= 0){
					itemSlot[3 + page] = 0;
					itemQuantity[3 + page] = 0;
					AutoSortItem();
				}
			}
			GUI.Label ( new Rect(125, 310, 320, 75), dataItem.usableItem[itemSlot[3 + page]].itemName.ToString() , itemNameText); //Item Name
			GUI.Label ( new Rect(125, 335, 320, 75), dataItem.usableItem[itemSlot[3 + page]].description.ToString() , itemDescriptionText); //Item Description
			if(itemQuantity[3 + page] > 0){
				GUI.Label ( new Rect(88, 358, 40, 30), itemQuantity[3 + page].ToString() , itemQuantityText); //Quantity
			}
			//------------------------------------------------------
			if (GUI.Button ( new Rect(30,390,75,75),dataItem.usableItem[itemSlot[4 + page]].icon)){
				UseItem(itemSlot[4 + page]);
				if(itemQuantity[4 + page] > 0){
					itemQuantity[4 + page]--;
				}
				if(itemQuantity[4 + page] <= 0){
					itemSlot[4 + page] = 0;
					itemQuantity[4 + page] = 0;
					AutoSortItem();
				}
			}
			GUI.Label ( new Rect(125, 400, 320, 75), dataItem.usableItem[itemSlot[4 + page]].itemName.ToString() , itemNameText); //Item Name
			GUI.Label ( new Rect(125, 425, 320, 75), dataItem.usableItem[itemSlot[4 + page]].description.ToString() , itemDescriptionText); //Item Description
			if(itemQuantity[4 + page] > 0){
				GUI.Label ( new Rect(88, 448, 40, 30), itemQuantity[4 + page].ToString() , itemQuantityText); //Quantity
			}
			//------------------------------------------------------
			
			if (GUI.Button (Rect (220,485,50,52), "1")) {
				page = 0;
			}
			if (GUI.Button (Rect (290,485,50,52), "2")) {
				page = itemPageMultiply;
			}
			if (GUI.Button (Rect (360,485,50,52), "3")) {
				page = itemPageMultiply *2;
			}
			
			GUI.Label ( new Rect(20, 505, 150, 50), "$ " + cash.ToString() , itemDescriptionText);
			//---------------------------
		}
		
		//---------------Equipment Tab----------------------------
		if(menu && equipMenu){
			//Close Window Button
			if (GUI.Button ( new Rect(390,8,70,70), "X")) {
				OnOffMenu();
			}
			//Weapon
			GUI.Label ( new Rect(20, 60, 150, 50), "Weapon");			
			if (GUI.Button ( new Rect(100,30,70,70), dataItem.equipment[weaponEquip].icon)){
				if(!allowWeaponUnequip || weaponEquip == 0){
					return;
				}
				UnEquip(weaponEquip);
			}
			//Armor
			GUI.Label ( new Rect(200, 60, 150, 50), "Armor");
			if (GUI.Button ( new Rect(260,30,70,70), dataItem.equipment[armorEquip].icon)){
				if(!allowArmorUnequip || armorEquip == 0){
					return;
				}
				UnEquip(armorEquip);
				
			}
			
			
			//--------Equipment Slot---------
			if (GUI.Button ( new Rect(30,130,75,75),dataItem.equipment[equipment[0 + page]].icon)){
				EquipItem(equipment[0 + page] , 0 + page);
			}
			GUI.Label ( new Rect(125, 140, 320, 75), dataItem.equipment[equipment[0 + page]].itemName.ToString() , itemNameText); //Item Name
			GUI.Label ( new Rect(125, 165, 320, 75), dataItem.equipment[equipment[0 + page]].description.ToString() , itemDescriptionText); //Item Description
			//------------------------------------------------------
			if (GUI.Button ( new Rect(30,220,75,75),dataItem.equipment[equipment[1 + page]].icon)){
				EquipItem(equipment[1 + page] , 1 + page);
			}
			GUI.Label ( new Rect(125, 230, 320, 75), dataItem.equipment[equipment[1 + page]].itemName.ToString() , itemNameText); //Item Name
			GUI.Label ( new Rect(125, 255, 320, 75), dataItem.equipment[equipment[1 + page]].description.ToString() , itemDescriptionText); //Item Description
			//------------------------------------------------------
			if (GUI.Button ( new Rect(30,310,75,75),dataItem.equipment[equipment[2 + page]].icon)){
				EquipItem(equipment[2 + page] , 2 + page);
			}
			GUI.Label ( new Rect(125, 320, 320, 75), dataItem.equipment[equipment[2 + page]].itemName.ToString() , itemNameText); //Item Name
			GUI.Label ( new Rect(125, 345, 320, 75), dataItem.equipment[equipment[2 + page]].description.ToString() , itemDescriptionText); //Item Description
			//------------------------------------------------------
			if (GUI.Button ( new Rect(30,400,75,75),dataItem.equipment[equipment[3 + page]].icon)){
				EquipItem(equipment[3 + page] , 3 + page);
			}
			GUI.Label ( new Rect(125, 410, 320, 75), dataItem.equipment[equipment[3 + page]].itemName.ToString() , itemNameText); //Item Name
			GUI.Label ( new Rect(125, 435, 320, 75), dataItem.equipment[equipment[3 + page]].description.ToString() , itemDescriptionText); //Item Description
			//------------------------------------------------------
			
			if (GUI.Button (Rect (220,485,50,52), "1")) {
				page = 0;
			}
			if (GUI.Button (Rect (290,485,50,52), "2")) {
				page = equipmentPageMultiply;
			}
			if (GUI.Button (Rect (360,485,50,52), "3")) {
				page = equipmentPageMultiply *2;
			}

			GUI.Label ( new Rect(20, 505, 150, 50), "$ " + cash.ToString() , itemDescriptionText);
			
		}
		GUI.DragWindow (new Rect (0,0,10000,10000)); 
	}

function AddItem(id : int,quan : int) : boolean{
	var full : boolean = false;
	var geta : boolean = false;
		
	var pt : int = 0;
	while(pt < itemSlot.Length && !geta){
		if(itemSlot[pt] == id){
			itemQuantity[pt] += quan;
			geta = true;
		}else if(itemSlot[pt] == 0){
			itemSlot[pt] = id;
			itemQuantity[pt] = quan;
			geta = true;
		}else{
			pt++;
			if(pt >= itemSlot.Length){
				full = true;
				print("Full");
			}
		}
		
	}
	
	return full;

}

function AddEquipment(id : int) : boolean{
	var full : boolean = false;
	var geta : boolean = false;
	
	
	var pt : int = 0;
	while(pt < equipment.Length && !geta){
		if(equipment[pt] == 0){
			equipment[pt] = id;
			geta = true;
		}else{
			pt++;
			if(pt >= equipment.Length){
				full = true;
				print("Full");
			}
		}
		
	}
	
	return full;

}
//------------AutoSort----------
function AutoSortItem(){
		var pt : int = 0;
		var nextp : int = 0;
		var clearr : boolean = false;
	while(pt < itemSlot.Length){
		if(itemSlot[pt] == 0){
			nextp = pt + 1;
			while(nextp < itemSlot.Length && !clearr){
				if(itemSlot[nextp] > 0){
				//Fine Next Item and Set
					itemSlot[pt] = itemSlot[nextp];
					itemQuantity[pt] = itemQuantity[nextp];
					itemSlot[nextp] = 0;
					itemQuantity[nextp] = 0;
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

function AutoSortEquipment(){
		var pt : int = 0;
		var nextp : int = 0;
		var clearr : boolean = false;
	while(pt < equipment.Length){
		if(equipment[pt] == 0){
			nextp = pt + 1;
			while(nextp < equipment.Length && !clearr){
				if(equipment[nextp] > 0){
				//Fine Next Item and Set
					equipment[pt] = equipment[nextp];
					equipment[nextp] = 0;
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


function OnOffMenu(){
	//Freeze Time Scale to 0 if Window is Showing
	if(!menu && Time.timeScale != 0.0){
			menu = true;
			Time.timeScale = 0.0;
			ResetPosition();
			Screen.lockCursor = false;
	}else if(menu){
			menu = false;
			Time.timeScale = 1.0;
			//Screen.lockCursor = true;
	}

}

function AssignWeaponAnimation(id : int){
	var dataItem : ItemData = database.GetComponent(ItemData);
	var playerAnim : PlayerAnimation = player.GetComponent(PlayerAnimation);
	if(!playerAnim){
		//If use Mecanim
		AssignMecanimAnimation(id);
		return;
	}
	
	//Assign All Attack Combo Animation of the weapon from Database
	if(dataItem.equipment[id].attackCombo && dataItem.equipment[id].EquipmentType == dataItem.equipment[id].EqType.Weapon){
  			var allPrefab : int = dataItem.equipment[id].attackCombo.length;
  			player.GetComponent(AttackTrigger).attackCombo = new AnimationClip[allPrefab];
  			player.GetComponent(AttackTrigger).c = 0;
  			
  			var a : int = 0;
  			if(allPrefab > 0){
   				while(a < allPrefab){
    				player.GetComponent(AttackTrigger).attackCombo[a] = dataItem.equipment[id].attackCombo[a];
    				player.GetComponent(AttackTrigger).mainModel.GetComponent.<Animation>()[dataItem.equipment[id].attackCombo[a].name].layer = 15;
    				a++;
   				}
  			}
  			player.GetComponent(AttackTrigger).whileAttack = parseInt(dataItem.equipment[id].whileAttack);
  			//Assign Attack Speed
  			player.GetComponent(AttackTrigger).attackSpeed = dataItem.equipment[id].attackSpeed;
  			player.GetComponent(AttackTrigger).atkDelay1 = dataItem.equipment[id].attackDelay;
 		}
		
	if(dataItem.equipment[id].idleAnimation){
		player.GetComponent(PlayerAnimation).idle = dataItem.equipment[id].idleAnimation;
	}
	if(dataItem.equipment[id].runAnimation){
		playerAnim.run = dataItem.equipment[id].runAnimation;
	}
	if(dataItem.equipment[id].rightAnimation){
		playerAnim.right = dataItem.equipment[id].rightAnimation;
	}
	if(dataItem.equipment[id].leftAnimation){
		playerAnim.left = dataItem.equipment[id].leftAnimation;
	}
	if(dataItem.equipment[id].backAnimation){
		playerAnim.back = dataItem.equipment[id].backAnimation;
	}
	if(dataItem.equipment[id].jumpAnimation){
		player.GetComponent(PlayerAnimation).jump = dataItem.equipment[id].jumpAnimation;
	}
	playerAnim.AnimationSpeedSet();

}

function AssignMecanimAnimation(id : int){
	var dataItem : ItemData = database.GetComponent(ItemData);
	if(dataItem.equipment[id].EquipmentType == dataItem.equipment[id].EqType.Weapon){
			player.GetComponent(AttackTrigger).whileAttack = parseInt(dataItem.equipment[id].whileAttack);
  			//Assign Attack Speed
  			player.GetComponent(AttackTrigger).attackSpeed = dataItem.equipment[id].attackSpeed;
  			player.GetComponent(AttackTrigger).atkDelay1 = dataItem.equipment[id].attackDelay;
  			//Set Weapon Type ID to Mecanim Animator and Set New Idle
  			player.GetComponent(PlayerMecanimAnimation).SetWeaponType(dataItem.equipment[id].weaponType , dataItem.equipment[id].idleAnimation.name);
  			
  			var allPrefab : int = dataItem.equipment[id].attackCombo.length;
  			player.GetComponent(AttackTrigger).attackCombo = new AnimationClip[allPrefab];
  
  			//Set Attack Animation
  			var a : int = 0;
  			if(allPrefab > 0){
   				while(a < allPrefab){
    				player.GetComponent(AttackTrigger).attackCombo[a] = dataItem.equipment[id].attackCombo[a];
    				a++;
   				}
  			}
	}

}

function ResetPosition(){
		//Reset GUI Position when it out of Screen.
		if(windowRect.x >= Screen.width -30 || windowRect.y >= Screen.height -30 || windowRect.x <= -70 || windowRect.y <= -70 ){
			windowRect = originalRect;
		}
}