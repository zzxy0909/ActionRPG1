#pragma strict
@HideInInspector
var mainModel : GameObject;

var useMecanim : boolean = false;
var attackPoint : Transform;
var attackPrefab : Transform;
enum whileAtk{
		MeleeFwd = 0,
		Immobile = 1,
		WalkFree = 2
}
var whileAttack : whileAtk = whileAtk.MeleeFwd;

class LockOn{
		var enable : boolean = true;
		var radius : float = 5.0;  //this is radius to checks for other objects
		var lockOnRange : float = 4.0; //this is how far it checks for other objects
		@HideInInspector
		var lockTarget : Transform;
		@HideInInspector
		var target : GameObject;
}
var autoLockTarget : LockOn;

class SkilAtk {
		var icon : Texture2D;
		var skillPrefab : Transform;
		var skillAnimation : AnimationClip;
		var skillAnimationSpeed : float = 1.0f;
		var manaCost : int = 10;
	}
var skill : SkilAtk[] = new SkilAtk[3];

private var atkDelay : boolean = false;
var freeze : boolean = false;

var attackSpeed : float = 0.15;
private var nextFire : float = 0.0;
var atkDelay1 : float = 0.1;
var skillDelay : float = 0.3;

var attackCombo : AnimationClip[] = new AnimationClip[3];
var attackAnimationSpeed : float = 1.0;

private var meleefwd : boolean = false;
@HideInInspector
var isCasting : boolean = false;
@HideInInspector
var c : int = 0;
private var conCombo : int = 0;

@HideInInspector
var Maincam : Transform;
var MaincamPrefab : GameObject;
var attackPointPrefab : GameObject;

private var str : int = 0;
private var matk : int = 0;

private var skillEquip : int  = 0;

class AtkSound{
		var attackComboVoice : AudioClip[] = new AudioClip[3];
		var magicCastVoice : AudioClip;
		var hurtVoice : AudioClip;
}
var sound : AtkSound;

function Awake () {
	gameObject.tag = "Player";
	mainModel = GetComponent(Status).mainModel;
	if(!mainModel){
		mainModel = this.gameObject;
	}
	GetComponent(Status).useMecanim = useMecanim;
	
	var oldcam : GameObject[] = GameObject.FindGameObjectsWithTag("MainCamera");
	for(var o : GameObject in oldcam){
		Destroy(o);
	}
	var newCam : GameObject = Instantiate(MaincamPrefab, transform.position , MaincamPrefab.transform.rotation);
    Maincam = newCam.transform;
    		
	str = GetComponent(Status).addAtk;
	matk = GetComponent(Status).addMatk;
	//Set All Attack Animation'sLayer to 15
	var animationSize : int = attackCombo.length;
	var a : int = 0;
	if(animationSize > 0 && !useMecanim){
		while(a < animationSize && attackCombo[a]){
			mainModel.GetComponent.<Animation>()[attackCombo[a].name].layer = 15;
			a++;
		}
	}
	
//--------------------------------
	//Spawn new Attack Point if you didn't assign it.
    if(!attackPoint){
    	if(!attackPointPrefab){
    		print("Please assign Attack Point");
    		freeze = true;
    		return;
    	}
    	var newAtkPoint : GameObject = Instantiate(attackPointPrefab, transform.position , transform.rotation);
    	newAtkPoint.transform.parent = this.transform;
    	attackPoint = newAtkPoint.transform;	
    }
    //GetComponent(Status).hurt = GetComponent(PlayerAnimation).hurt;
    if(sound.hurtVoice){
			GetComponent(Status).hurtVoice = sound.hurtVoice;
	}
}


function Update () {
	var stat : Status = GetComponent(Status);
	if(freeze || atkDelay || Time.timeScale == 0.0 || stat.freeze){
		return;
	}
	var controller : CharacterController = GetComponent(CharacterController);
	if (stat.flinch){
		controller.Move(stat.knock * 6* Time.deltaTime);
		return;
	}
		
	if (meleefwd){
		var lui : Vector3 = transform.TransformDirection(Vector3.forward);
		controller.Move(lui * 5 * Time.deltaTime);
	}
	var bulletShootout : Transform;
//----------------------------
	//Normal Trigger
		if (Input.GetKey("j") && Time.time > nextFire && !isCasting) {
			TriggerAttack();
		}
		//Magic
		if(Input.GetKeyDown("1") && !isCasting && skill[0].skillPrefab){
			MagicSkill(0);
		}
		if(Input.GetKeyDown("2") && !isCasting && skill[1].skillPrefab){
			MagicSkill(1);
		}
		if(Input.GetKeyDown("3") && !isCasting && skill[2].skillPrefab){
			MagicSkill(2);
		}
		
}

function TriggerAttack(){
		if(freeze || atkDelay || Time.timeScale == 0.0 || GetComponent(Status).freeze){
			return;
		}
		if (Time.time > nextFire && !isCasting) {
			if(Time.time > (nextFire + 0.5)){
				c = 0;
			}
		//Attack Combo
			if(attackCombo.Length >= 1){
				conCombo++;
				AttackCombo();
			}
		}
}

function TriggerSkill(sk : int){
		if(freeze || atkDelay || Time.timeScale == 0.0 || GetComponent(Status).freeze){
			return;
		}
		if (Time.time > nextFire && !isCasting && skill[sk].skillPrefab) {
			MagicSkill(sk);
		}

}

function AttackCombo(){
	if(!attackCombo[c]){
		print("Please assign attack animation in Attack Combo");
		return;
	}
	if(autoLockTarget.enable){
		LockOnEnemy();
	}
	str = GetComponent(Status).addAtk;
	matk = GetComponent(Status).addMatk;
	var bulletShootout : Transform;
	isCasting = true;
	// If Melee Dash
	if(whileAttack == whileAtk.MeleeFwd){
			GetComponent(CharacterMotor).canControl = false;
			MeleeDash();
	}
	// If Immobile
	if(whileAttack == whileAtk.Immobile){
			GetComponent(CharacterMotor).canControl = false;
	}
	if(sound.attackComboVoice.Length > c && sound.attackComboVoice[c]){
			GetComponent.<AudioSource>().clip = sound.attackComboVoice[c];
			GetComponent.<AudioSource>().Play();
		}
	while(conCombo > 0){
		if(!useMecanim){
			//For Legacy Animation
			mainModel.GetComponent.<Animation>().PlayQueued(attackCombo[c].name, QueueMode.PlayNow).speed = attackAnimationSpeed;
			var wait : float = mainModel.GetComponent.<Animation>()[attackCombo[c].name].length;
		}else{
			//For Mecanim Animation
			GetComponent(PlayerMecanimAnimation).AttackAnimation(attackCombo[c].name);
			var clip = GetComponent(PlayerMecanimAnimation).animator.GetCurrentAnimatorClipInfo(0);
			wait = clip.Length -0.5;
		}
	
	yield WaitForSeconds(atkDelay1);
	c++;
	
	nextFire = Time.time + attackSpeed;
			bulletShootout = Instantiate(attackPrefab, attackPoint.transform.position , attackPoint.transform.rotation);
			bulletShootout.GetComponent(BulletStatus).Setting(str , matk , "Player" , this.gameObject);
			conCombo -= 1;
			
	if(c >= attackCombo.Length){
		c = 0;
		atkDelay = true;
		yield WaitForSeconds(wait);
		atkDelay = false;
	}else{
		yield WaitForSeconds(attackSpeed);
	}
	
	}
	
	//yield WaitForSeconds(attackSpeed);
	isCasting = false;
	GetComponent(CharacterMotor).canControl = true;
}

function MeleeDash(){
	meleefwd = true;
	yield WaitForSeconds(0.2);
	meleefwd = false;

}

//---------------------
//-------
function MagicSkill(skillID : int){
	if(!skill[skillID].skillAnimation){
		print("Please assign skill animation in Skill Animation");
		return;
	}
	if(autoLockTarget.enable){
		LockOnEnemy();
	}
	str = GetComponent(Status).addAtk;
	matk = GetComponent(Status).addMatk;
	
	if(GetComponent(Status).mana < skill[skillID].manaCost || GetComponent(Status).silence){
		return;
	}
	if(sound.magicCastVoice){
		GetComponent.<AudioSource>().clip = sound.magicCastVoice;
		GetComponent.<AudioSource>().Play();
	}
	isCasting = true;
	GetComponent(CharacterMotor).canControl = false;
	
	if(!useMecanim){
		//For Legacy Animation
		mainModel.GetComponent.<Animation>()[skill[skillID].skillAnimation.name].layer = 16;
		mainModel.GetComponent.<Animation>()[skill[skillID].skillAnimation.name].speed = skill[skillID].skillAnimationSpeed;
		mainModel.GetComponent.<Animation>().Play(skill[skillID].skillAnimation.name);
		var wait : float = mainModel.GetComponent.<Animation>()[skill[skillID].skillAnimation.name].length -0.3;
	}else{
		//For Mecanim Animation
		GetComponent(PlayerMecanimAnimation).AttackAnimation(skill[skillID].skillAnimation.name);
		var clip = GetComponent(PlayerMecanimAnimation).animator.GetCurrentAnimatorClipInfo(0);
		wait = clip.Length -0.3;
	}
		
	nextFire = Time.time + skillDelay;
	//Maincam.GetComponent(ARPGcamera).lockOn = true;
	var bulletShootout : Transform;
	
	yield WaitForSeconds(wait);
	//Maincam.GetComponent(ARPGcamera).lockOn = false;
	bulletShootout = Instantiate(skill[skillID].skillPrefab, attackPoint.transform.position , attackPoint.transform.rotation);
	bulletShootout.GetComponent(BulletStatus).Setting(str , matk , "Player" , this.gameObject);
	yield WaitForSeconds(skillDelay);
	isCasting = false;
	GetComponent(CharacterMotor).canControl = true;
	GetComponent(Status).mana -= skill[skillID].manaCost;
}

// Lock On the closest enemy 
function LockOnEnemy() { 
    var checkPos : Vector3 = transform.position + transform.forward * autoLockTarget.lockOnRange;
    var closest : GameObject; 
    
    var distance : float = Mathf.Infinity; 
    var position : Vector3 = transform.position; 
       autoLockTarget.lockTarget = null; // Reset Lock On Target
       var objectsAroundMe : Collider[] = Physics.OverlapSphere(checkPos , autoLockTarget.radius);
        for(var obj : Collider in objectsAroundMe){
            if(obj.CompareTag("Enemy")){
               var diff : Vector3 = (obj.transform.position - position); 
		       var curDistance : float = diff.sqrMagnitude; 
		       if (curDistance < distance) { 
		       //------------
		         closest = obj.gameObject; 
		         distance = curDistance;
		         autoLockTarget.target = closest;
		         autoLockTarget.lockTarget = closest.transform;
		       } 
            }
        }
        //Face to the target
        if(autoLockTarget.lockTarget){
				var lookOn : Vector3 = autoLockTarget.lockTarget.position;
		     	lookOn.y = transform.position.y;
		  		transform.LookAt(lookOn);
		}
    
}

@script RequireComponent (Status)
@script RequireComponent (StatusWindow)
@script RequireComponent (HealthBar)
//@script RequireComponent (PlayerAnimation)
@script RequireComponent (CharacterMotor)
@script RequireComponent (Inventory)
@script RequireComponent (QuestStat)
@script RequireComponent (SkillWindow)
@script RequireComponent (SaveLoad)
@script RequireComponent (DontDestroyOnload)
@script RequireComponent (SpawnPartner)
@script RequireComponent (ShowEnemyHealth)
@script RequireComponent (TopDownController)