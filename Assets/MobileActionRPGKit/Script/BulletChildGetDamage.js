#pragma strict
var master : GameObject;

function Start () {
	GetComponent(BulletStatus).totalDamage = master.GetComponent(BulletStatus).totalDamage;
	GetComponent(BulletStatus).shooterTag = master.GetComponent(BulletStatus).shooterTag;
	GetComponent(BulletStatus).shooter = master.GetComponent(BulletStatus).shooter;
	
}
