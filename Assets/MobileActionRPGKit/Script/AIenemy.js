#pragma strict

enum AIState { Moving = 0, Pausing = 1 , Idle = 2 , Patrol = 3}

private var mainModel : GameObject;
var useMecanim : boolean = false;
var animator : Animator; //For Mecanim
var followTarget : Transform;
var approachDistance  : float = 2.0f;
var detectRange : float = 15.0f;
var lostSight : float = 100.0f;
var speed  : float = 4.0f;
var movingAnimation : AnimationClip;
var idleAnimation : AnimationClip;
var attackAnimation : AnimationClip;
var hurtAnimation : AnimationClip;

@HideInInspector
var flinch : boolean = false;

var freeze : boolean = false;

var bulletPrefab : Transform;
var attackPoint : Transform;

var attackCast : float = 0.3;
var attackDelay : float = 0.5;

var followState : AIState = AIState.Idle;
private var distance : float = 0.0;
private var atk : int = 0;
private var matk : int = 0;
private var knock : Vector3 = Vector3.zero;
@HideInInspector
var cancelAttack : boolean = false;
private var attacking : boolean = false;
private var castSkill : boolean = false;
private var gos : GameObject[]; 

var attackVoice : AudioClip;
var hurtVoice : AudioClip;

function Start () {
	gameObject.tag = "Enemy"; 
	if(!attackPoint){
		attackPoint = this.transform;
	}
	mainModel = GetComponent(Status).mainModel;
	if(!mainModel){
		mainModel = this.gameObject;
	}
	GetComponent(Status).useMecanim = useMecanim;
	//Assign MainModel in Status Script
	GetComponent(Status).mainModel = mainModel;
		//Set ATK = Monster's Status
		atk = GetComponent(Status).atk;
		matk = GetComponent(Status).matk;
        
      	followState = AIState.Idle;
        
        if(!useMecanim){
      		//If using Legacy Animation
	      	mainModel.GetComponent.<Animation>().Play(idleAnimation.name);
       		mainModel.GetComponent.<Animation>()[hurtAnimation.name].layer = 10;
	        GetComponent(Status).hurt = hurtAnimation;
        }else{
        	//If using Mecanim Animation
        	if(!animator){
				animator = mainModel.GetComponent(Animator);
			}
        }
        
		if(hurtVoice){
			GetComponent(Status).hurtVoice = hurtVoice;
		}
}

function GetDestination()
    {
        var destination : Vector3 = followTarget.position;
        destination.y = transform.position.y;
        return destination;
    }

function Update () {
	var stat : Status = GetComponent(Status);
	var controller : CharacterController = GetComponent(CharacterController);
	
	gos = GameObject.FindGameObjectsWithTag("Player"); 
    if (gos.Length > 0) {
			followTarget = FindClosest().transform;
	}
	if(useMecanim){
		animator.SetBool("hurt" , stat.flinch);
	}
	
	if (stat.flinch){
		cancelAttack = true;
		var knock = transform.TransformDirection(Vector3.back);
		controller.Move(knock * 5* Time.deltaTime);
		followState = AIState.Moving;
		return;
	}
	
	if(freeze || stat.freeze){
		return;
	}

	if(!followTarget){
		return;
	}
	//-----------------------------------
	
		if (followState == AIState.Moving) {
			//mainModel.animation.CrossFade(movingAnimation.name, 0.2f);
				if(!useMecanim){
	            //If using Legacy Animation
	                mainModel.GetComponent.<Animation>().CrossFade(movingAnimation.name, 0.2f);
	            }else{
					animator.SetBool("run" , true);
				}
            if ((followTarget.position - transform.position).magnitude <= approachDistance) {
                followState = AIState.Pausing;
                //----Attack----
                	Attack();
            }else if ((followTarget.position - transform.position).magnitude >= lostSight)
            {//Lost Sight
            	GetComponent(Status).health = GetComponent(Status).maxHealth;
            	//mainModel.animation.CrossFade(idleAnimation.name, 0.2f);
            	if(!useMecanim){
                //If using Legacy Animation
                	mainModel.GetComponent.<Animation>().CrossFade(idleAnimation.name, 0.2f); 
                }else{
					animator.SetBool("run" , false);
				}
                followState = AIState.Idle;
            }else {
                var forward : Vector3 = transform.TransformDirection(Vector3.forward);
     			controller.Move(forward * speed * Time.deltaTime);
     			   
     			   var destiny : Vector3 = followTarget.position;
     			   destiny.y = transform.position.y;
  				   transform.LookAt(destiny);
            }
        }
        else if (followState == AIState.Pausing){
        		//mainModel.animation.CrossFade(idleAnimation.name, 0.2f);
        		if(!useMecanim){
                //If using Legacy Animation
                	mainModel.GetComponent.<Animation>().CrossFade(idleAnimation.name, 0.2f); 
                }else{
					animator.SetBool("run" , false);
				}
       			 var destinya : Vector3 = followTarget.position;
     			   destinya.y = transform.position.y;
  				   transform.LookAt(destinya);
  				   			   
            distance = (transform.position - GetDestination()).magnitude;
            if (distance > approachDistance) {
                followState = AIState.Moving;
            }
        }
        //----------------Idle Mode--------------
        else if (followState == AIState.Idle){
        	//mainModel.animation.CrossFade(idleAnimation.name, 0.2f);
  			var destinyheight : Vector3 = followTarget.position;
     			destinyheight.y = transform.position.y - destinyheight.y;
     		var getHealth : int = GetComponent(Status).maxHealth - GetComponent(Status).health;
     			
            distance = (transform.position - GetDestination()).magnitude;
            if (distance < detectRange && Mathf.Abs(destinyheight.y) <= 4 || getHealth > 0){
                followState = AIState.Moving;
            }
        }
//-----------------------------------
}

function Attack(){
	cancelAttack = false;
	if(GetComponent(Status).flinch || GetComponent(Status).freeze || freeze || attacking){
		return;
	}
		freeze = true;
		attacking = true;
		if(!useMecanim){
        	//If using Legacy Animation
			mainModel.GetComponent.<Animation>().Play(attackAnimation.name);
		}else{
			animator.Play(attackAnimation.name);
		}
		yield WaitForSeconds(attackCast);
		if(GetComponent(Status).flinch){
			freeze = false;
			attacking = false;
			return;
		}
		//attackPoint.transform.LookAt(followTarget);
		if(!cancelAttack){
			if(attackVoice){
				GetComponent.<AudioSource>().clip = attackVoice;
				GetComponent.<AudioSource>().Play();
			}
			var bulletShootout : Transform = Instantiate(bulletPrefab, attackPoint.transform.position , attackPoint.transform.rotation);
			bulletShootout.GetComponent(BulletStatus).Setting(atk , matk , "Enemy" , this.gameObject);
		}

		yield WaitForSeconds(attackDelay);
		freeze = false;
		attacking = false;
		//mainModel.animation.CrossFade(movingAnimation.name, 0.2f);
		CheckDistance();
	
}

function CheckDistance(){
	if(!followTarget){
		if(!useMecanim){
	        //If using Legacy Animation
			mainModel.GetComponent.<Animation>().CrossFade(idleAnimation.name, 0.2f);  
		}else{
			animator.SetBool("run" , false);
		}
		followState = AIState.Idle;
		return;
	}
	var distancea : float = (followTarget.position - transform.position).magnitude;
	if (distancea <= approachDistance){
			var destinya : Vector3 = followTarget.position;
     		 destinya.y = transform.position.y;
  			 transform.LookAt(destinya);
              Attack();
     }else{
          followState = AIState.Moving;
          if(!useMecanim){
          //If using Legacy Animation
          	mainModel.GetComponent.<Animation>().CrossFade(movingAnimation.name, 0.2f);
          }else{
			animator.SetBool("run" , true);
		  }
       }
}


function FindClosest() : GameObject { 
    // Find Closest Player   
   // var gos : GameObject[]; 
    gos = GameObject.FindGameObjectsWithTag("Player");
    gos += GameObject.FindGameObjectsWithTag("Ally"); 
    if(!gos){
    	return;
    }
    var closest : GameObject; 
    
    var distance : float = Mathf.Infinity; 
    var position : Vector3 = transform.position; 

    for (var go : GameObject in gos) { 
       var diff : Vector3 = (go.transform.position - position); 
       var curDistance : float = diff.sqrMagnitude; 
       if (curDistance < distance) { 
       //------------
         closest = go; 
         distance = curDistance; 
       } 
    } 
  //  var target = closest;
    return closest; 
}

function UseSkill(skill : Transform , castTime : float , delay : float , anim : String , dist : float){
	cancelAttack = false;
	if(flinch || !followTarget || (followTarget.position - transform.position).magnitude >= dist || GetComponent(Status).silence || GetComponent(Status).freeze  || castSkill){
		return;
	}
	
		freeze = true;
		castSkill = true;
		if(!useMecanim){
	        //If using Legacy Animation
			mainModel.GetComponent.<Animation>().Play(anim);
		}else{
			animator.Play(anim);
		}
		yield WaitForSeconds(castTime);
		if(flinch){
			freeze = false;
			castSkill = false;
			return;
		}
		//attackPoint.transform.LookAt(followTarget);
		if(!cancelAttack){
			var bulletShootout : Transform = Instantiate(skill, attackPoint.transform.position , attackPoint.transform.rotation);
			bulletShootout.GetComponent(BulletStatus).Setting(atk , matk , "Enemy" , this.gameObject);
		}

		yield WaitForSeconds(delay);
		freeze = false;
		castSkill = false;
		if(!useMecanim){
	        //If using Legacy Animation
			mainModel.GetComponent.<Animation>().CrossFade(movingAnimation.name, 0.2f);
		}else{
			animator.SetBool("run" , true);
		}
	
}

@script RequireComponent (Status)
@script RequireComponent (CharacterMotor)